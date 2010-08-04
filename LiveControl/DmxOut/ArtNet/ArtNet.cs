using System.Text;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace Kevsoft.LiveControl.IDmxOut.ArtNet
{
    // 'Art-Net' Version 1.2 (c)Hippy 2005 (rowanmac@optusnet.com.au)
    // http://members.westnet.com.au/rowanmac/Artnet-dot-net-1.2.zip
    // Converted by Kevin Smith in to C#.Net from VB

    class ArtNet
    {
        #region "Declares"

        #region "Network"
        // network 
        private UdpClient UDP_Server;
        private UdpClient UDP_Client = new UdpClient();
        // recieve thread 
        private Thread RecieveThread;
        // raised if a network error occurs
        private event NetworkErrorEventHandler NetworkError;
        private delegate void NetworkErrorEventHandler(string ErrorStr);
        // raised apon UDP reception
        private event DataArrivalEventHandler DataArrival;
        private delegate void DataArrivalEventHandler(byte[] data);

        #endregion

        #region "Art-Net"
        // Art-Net constants
        private const short OemUser = 0xff;
        // Reserved for Unknown user/manufact
        private const byte ProtocolVersion = 14;
        private const short DefaultPort = 6454;
        // network port for Art-Net

        // packet types
        private const short OpPoll = 0x2000;
        //Poll */
        private const short OpPollReply = 0x2100;
        //ArtPollReply */
        private const short OpPollFpReply = 0x2200;
        //Reply from Four-Play */
        private const short OpOutput = 0x5000;
        //Output */
        private const short OpAddress = 0x6000;
        //Program Node Settings */
        private const short OpInput = 0x7000;
        //Setup DMX input enables */

        private const short StyleNode = 0;
        // Responder is a Node (DMX <-> Ethernet Device)
        private const short StyleServer = 1;
        // Lighting console or similar

        private const short MaxNumPorts = 1;
        //4
        private const short MaxExNumPorts = 32;
        private const short ShortNameLength = 18;
        private const short LongNameLength = 64;
        private const short PortNameLength = 32;
        private const short MaxDataLength = 512 - 1;
        // 0..511


        // These are used in creation of packets, they are block copied into 
        // place to gain even a tiny bit more speed...

        // Ascii "Art-Net" & 0
        private byte[] ArtNetHead = { 65, 114, 116, 45, 78, 101, 116, 0 };

        // Addr... IP address (4 byte) and port (2 byte) of our node
        private byte[] ArtAddr = { 127, 0, 0, 1, LoByte(DefaultPort), HiByte(DefaultPort) };

        private string ArtShortName = "Art-Net Node";
        private string ArtLongName = "PC Lighting Application - Art-Net-dot-Net V1.2 (c) hippy '05";
        private string ArtNodeReport = "Working normally i think. I'll go and have a look...";

        // true if artnet has been detected
        private bool ArtDetected;

        #endregion

        #region "DMX"
        // this event is raised when a art-net dmx packet is recieved
        public event DMX_RecievedEventHandler DMX_Recieved;
        public delegate void DMX_RecievedEventHandler(int Universe, byte[] DMX);
        //Public Event DMX_Updated(ByVal Universe As Integer)

        #endregion

        #endregion


        #region "Public Interface"

        // note: this broadcasts to everyone :)
        // Public BroadcastAddress as String = "2.255.255.255"
        // this is the address Art-Net packets will be sent to
        public string BroadcastAddress = "255.255.255.255";

        // has art-net been recieved in this session
        public bool Detected
        {
            get { return ArtDetected; }
        }
        // set/retrieve the ShortName of our Node
        public string ShortName
        {
            get { return ArtShortName; }
            set { ArtShortName = value.Substring(0,ShortNameLength); }
        }
        // set/retrieve the LongName of our Node
        public string LongName
        {
            get { return ArtLongName; }
            set { ArtLongName = value.Substring(0, LongNameLength); }
        }



        // send a DMX packet to 'Universe', dmx is in a byte array 'Data' and the DataLength also is required
        public void SendDMX(Int16 Universe, byte[] Data, Int16 DataLength)
        {
            // check the param supplied
            // msgbox's are used because they should never invaild, you must get it write!
            if (DataLength > 512)
            {
                throw new Exception("Art-Net> Universe '" + Universe + "' larger than 512 bytes");
           }
            else if (DataLength < 0)
            {
                throw new Exception("Art-Net> Universe '" + Universe + "' Invalid");
            }

            // this constant defines the length in bytes of the header info of the 
            // OpOutput packet
            const int HeaderLength = 17;

            // prepare a buffer of the correct length
            byte[] buf = new byte[HeaderLength + DataLength + 1];

            // copy the header 'Art-Net ' into the buffer
            Buffer.BlockCopy(ArtNetHead, 0, buf, 0, ArtNetHead.Length);

            // op code 
            buf[8] = LoByte(OpOutput);
            // dmx output
            buf[9] = HiByte(OpOutput);

            // version (two bytes)
            buf[10] = 0;
            buf[11] = ProtocolVersion;
            //sequence
            buf[12] = 0;
            //physical 
            buf[13] = 0;
            // universe (two bytes)
            buf[14] = LoByte(Universe);
            buf[15] = HiByte(Universe);
            // data length (two bytes, note: manual byte-swap!)
            buf[16] = HiByte(DataLength);
            // data length 1 - 512
            buf[17] = LoByte(DataLength);
            // data length

            try
            {
                // now copy in the dmx data
                Buffer.BlockCopy(Data, 0, buf, 18, DataLength);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("blockcopy failure! " + ex.Message);
                return;
            }

            // broadcast the newly formed Art-Net packet
            UDP_Send(BroadcastAddress, DefaultPort, buf);
        }


        #endregion


        #region "Management"

        // this recieves and processes data from the network
        private void ArtRecieve(byte[] Data)
        {
            short x = 0;


            Int16 OpCode = default(Int16);

            try
            {


                // check for art-net header, exit if not an 'Art-Net ' packet
                for (x = 0; x <= ArtNetHead.Length - 1; x++)
                {
                    if (Data[x] != ArtNetHead[x]) return;

                }

                ArtDetected = true;

                // determine the OpCode of the packet
                OpCode = MakeInt16(Data[8], Data[9]);

                // do something with the packet
                switch (OpCode)
                {

                    case OpOutput:
                        // we recieved an Art-Net DMX packet

                        byte VerH = Data[10];
                        byte Ver = Data[11];

                        // chech version
                        if (Ver != ProtocolVersion)
                        {
                            Debug.WriteLine("Possible Version Conflict!");
                        }


                        byte Sequence = Data[12];
                        // not used in this app
                        byte Physical = Data[13];
                        // reserved

                        //where the dmx is destined
                        Int16 Universe = MakeInt16(Data[15], Data[14]);
                        // the length of the dmx data
                        Int16 Length = MakeInt16(Data[17], Data[16]);

                        byte[] buf = new byte[513];
                        // temp buffer

                        // get the dmx data
                        for (x = 0; x <= Length; x++)
                        {
                            buf[x] = Data[x + 17];
                        }


                        // we have recieved a dmx packet
                        if (DMX_Recieved != null)
                        {
                            DMX_Recieved(Universe, buf);
                        }

                        break;


                    case OpPoll:
                        // Poll for nodes, 
                        Send_ArtPollReply();
                        // send a reply...
                        Debug.WriteLine("Recieved Poll, Sent Reply");
                        break;

                    case OpPollReply:
                        Debug.WriteLine("Recieved Art Poll Reply");
                        break;


                    default:
                        Debug.WriteLine("Unknown/NYI OpCode: " + OpCode);
                        break;
                }
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        // send a 'ArtPoll', any node should respond with a ArtPollReply
        private void Send_ArtPoll()
        {

            const byte HeaderLength = 13;

            // prepare a buffer of the correct length
            byte[] buf = new byte[HeaderLength + 1];

            // copy the header 'Art-Net ' into the buffer
            Buffer.BlockCopy(ArtNetHead, 0, buf, 0, ArtNetHead.Length);

            // op code 
            buf[8] = LoByte(OpPoll);
            // OpPoll Artnet Poll Request
            buf[9] = HiByte(OpPoll);

            // version (two bytes)
            buf[10] = 0;
            buf[11] = ProtocolVersion;

            //  TalkToMe As Byte  ' bit 0 = not used

            //   Prev def was bit 0 = 0 if reply is broadcast
            //        bit 0 = 1 if reply is to server IP
            // All replies are noe broadcast as this feature caused too many
            // tech support calls
            // bit 1 = 0 then Node only replies when polled
            // bit 1 = 1 then Node sends reply when it needs to
            buf[12] = 0;

            // pad As Byte
            buf[13] = 0;

            // broadcast the newly formed Art-Net packet
            UDP_Send(BroadcastAddress, DefaultPort, buf);

        }

        // send a 'ArtPollReply' packet,in response to an ArtPoll
        private void Send_ArtPollReply()
        {
            const byte HeaderLength = 239;
            // total length of this packet

            // prepare a buffer of the correct length
            byte[] buf = new byte[HeaderLength + 1];

            // copy the header 'Art-Net ' into the buffer
            Buffer.BlockCopy(ArtNetHead, 0, buf, 0, ArtNetHead.Length);

            // op code 
            buf[8] = LoByte(OpPollReply);
            // OpPoll Artnet Poll Request
            buf[9] = HiByte(OpPollReply);

            // Address of node (us) (4 bytes (IP) + 2 bytes (port))
            // copy our pre-prepared address into the buffer
            Buffer.BlockCopy(ArtAddr, 0, buf, 10, ArtAddr.Length);

            // The node's current FIRMWARE VERS lo
            buf[16] = 1;
            //VersionInfoH As Byte   
            buf[17] = 1;
            //VersionInfo As Byte  

            //SubSwitchH As Byte     ' 0 - not used yet
            //'subswitch As Byte      ' from switch on front panel (0-15)
            buf[18] = 0;
            buf[19] = 0;
            // SUB-NET  (0-15)

            //Oem As Integer
            buf[20] = HiByte(OemUser);
            buf[21] = LoByte(OemUser);

            //UbeaVersion As Byte   ' Firmware version of UBEA
            buf[22] = 0;

            //Status  As Byte
            buf[23] = 0;

            //EstaMan  As Integer        ' Reserved for ESTA manufacturer id lo, zero for now
            buf[24] = 0;
            buf[25] = 0;

            byte x = 0;

            // build the names, and the node report...
            for (x = 26; x <= 26 + ArtShortName.Length - 1; x++)
            {
                buf[x] =Convert.ToByte(ArtShortName.ToCharArray(x - 26, 1));
            }
            for (x = 44; x <= 44 + ArtLongName.Length - 1; x++)
            {
                buf[x] = Convert.ToByte(ArtLongName.ToCharArray(x - 44, 1));
            }
            for (x = 108; x <= 108 + ArtNodeReport.Length - 1; x++)
            {
                buf[x] = Convert.ToByte(ArtNodeReport.ToCharArray(x - 108, 1));
            }

            // number of ports supported
            buf[172] = 0;
            // hi
            buf[173] = 4;
            // low

            //PortTypes(1 To MaxNumPorts) As Byte
            buf[174] = 0;
            // port 1 type 
            buf[175] = 0;
            // port 2 type
            buf[176] = 0;
            // port 3 type
            buf[177] = 0;
            // port 4 type

            //GoodInput(1 To MaxNumPorts) As Byte
            buf[178] = 0;
            buf[179] = 0;
            buf[180] = 0;
            buf[181] = 0;

            //GoodOutput(1 To MaxNumPorts)
            buf[182] = 0;
            buf[183] = 0;
            buf[184] = 0;
            buf[185] = 0;

            //Swin(1 To MaxNumPorts)  As Byte
            buf[186] = 0;
            buf[187] = 0;
            buf[188] = 0;
            buf[189] = 0;

            //Swout(1 To MaxNumPorts)   As Byte
            buf[190] = 0;
            buf[191] = 0;
            buf[192] = 0;
            buf[193] = 0;

            //SwVideo    As Byte
            buf[194] = 0;

            //SwMacro  As Byte
            buf[195] = 0;

            //SwRemote   As Byte
            buf[196] = 0;

            //Spare1    As Byte             ' Spare, currently zero
            //Spare2    As Byte             ' Spare, currently zero
            //Spare3    As Byte              ' Spare, currently zero
            for (x = 197; x <= 199; x++)
            {
                buf[x] = 0;
            }

            //Style     As Byte               ' Set to Style code to describe type of equipment
            buf[200] = (byte) StyleNode;
            // StyleServer

            //Mac(1 To 6) As Byte               ' Mac Address, zero if info not available
            buf[201] = 0;
            buf[202] = 0;
            buf[203] = 0;
            buf[204] = 0;
            buf[205] = 0;
            buf[206] = 0;

            //Filler(1 To 32)   As Byte              ' Filler bytes, currently zero.
            for (x = 207; x <= 207 + 32; x++)
            {
                buf[x] = x;
            }

            buf[239] = 255;
            // That's it!

            // broadcast the newly formed Art-Net packet
            UDP_Send(BroadcastAddress, DefaultPort, buf);
        }

        #endregion

        #region "Network"

        // start a thread to listen for UDP packets
        private bool UDP_Listen(int Port)
        {
            try
            {

                UDP_Server = new UdpClient(Port);
                // listen on port for UDP

                RecieveThread = new Thread(UDP_Recieve);
                // create a recieve thread
                RecieveThread.Start();
                // start a recieve thread
                return true;
            }
            catch (Exception err)
            {
                if (NetworkError != null)
                {
                    NetworkError(err.ToString());
                }
            }
            return false;
        }

        // send a byte array
        private void UDP_Send(string Host, int Port, byte[] Data)
        {
            try
            {
                UDP_Client.Connect(Host, Port);
                UDP_Client.Send(Data, Data.Length);
            }
            catch (Exception err)
            {
                if (NetworkError != null)
                {
                    NetworkError(err.ToString());
                }
            }
        }

        private void UDP_Recieve()
        {
            string LocalIP = null;

            // get the local ip
            String LocalHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(LocalHostName);
            IPAddress[] addr = ipEntry.AddressList;
            if(addr.Length>0)
                LocalIP = addr[0].ToString();

            do
            {
                try
                {

                    // store the remote ip 
                    IPEndPoint RemoteIP_EP = new IPEndPoint(IPAddress.Broadcast, 0);

                    // wait for data from the socket
                    byte[] data = UDP_Server.Receive(ref RemoteIP_EP);

                    // only process packets not from us, or else you get feedback :)
                    if (LocalIP.ToString() != RemoteIP_EP.Address.ToString())
                    {
                        if (DataArrival != null)
                        {
                            DataArrival(data);
                        }
                    }

                    Thread.Sleep(0);
                    // chill for a bit
                }
                catch (Exception err)
                {
                    if (NetworkError != null)
                    {
                        NetworkError(err.ToString());
                    }
                }
            }
            while (true);
        }

        private void CloseSocket()
        {
            try
            {
                UDP_Server.Close();
                RecieveThread.Abort();
            }
            catch (Exception err)
            {
                if (NetworkError != null)
                {
                    NetworkError(err.ToString());
                }
            }
        }
        #endregion

        #region "Byte Functions"

        // break an int16 into it's lo and hi bytes
        private static byte HiByte(int wParam)
        {
            return (byte)(wParam / 0x100 & 0xffL);
        }
        private static byte LoByte(int wParam)
        {
            return (byte)(wParam & 0xffL);
        }
        // perform a byte-swap
        private short End16(short iNum)
        {
            short iRes = 0;
            iRes = (short)((short)iNum & ((short)(0xff * Math.Pow(2, 8))));
            iRes = (short)(iRes | (short)iNum & 0xff00 / 256);
            return (short)(iRes - (iRes > 32767 ? 65536 : 0));
        }
        // make an integer from two bytes 
        private Int16 MakeInt16(byte lsb, byte msb)
        {
            Int16 newnum = default(Int16);
            newnum = msb;
            newnum = (Int16)(newnum << 8);
            newnum = (Int16)(newnum + lsb);
            return newnum;
        }

        #endregion

        #region "New() & Finalize()"

        public ArtNet(bool ListenForIncommingData)
        {
            // add a handler to the DataArrival event to the ArtRecieve Sub
            DataArrival += ArtRecieve;
            // start listening for Art-Net packets
            if (ListenForIncommingData == true) UDP_Listen(6454);
        }

        ~ArtNet()
        {
            CloseSocket();
            // close the socket!
        }
    }

        #endregion
}