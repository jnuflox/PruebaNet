using System;
using System.Text;

namespace Claro.Seguridad.Criptografia
{
    public class DES
    {
        #region [ Fields ]

        //Buffered key value
        private string _strKey;

        //Key-dependant
        private byte[,] _arrKey;

        //Values given in the DES standard
        private byte[] _arrInitialPermutation
                        , _arrPermutedChoice1
                        , _arrPermutedChoice2
                        , _arrFinalPermutation
                        , _arrEmptyArray
                        , _arrLeftShifts;

        private object[, , , , , ,] m_sBox = new object[8, 2, 2, 2, 2, 2, 2];

        #endregion

        #region [ Constructor ]

        public DES()
        {
            //Initialize the _key array;
            this._arrKey = new byte[48, 16];

            //Initialize the m_EmptyArray array
            this._arrEmptyArray = new byte[64];

            this.InitializeLeftShits();
            this.InitializeInitialPermutation();
            this.InitializePermutedChoice1();
            this.InitializePermutedChoice2();
            this.InitializeFinalPermutation();

            //Initialize the eight s-boxes
            byte[][] vSbox = new byte[8][]
            {
                new byte[] { 14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7, 0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8, 4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0, 15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13 }
                , new byte[] { 15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10, 3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5, 0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15, 13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9 }
                , new byte[] { 10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8, 13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1, 13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7, 1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12 }
                , new byte[] { 7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15, 13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9, 10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4, 3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14 }
                , new byte[] { 2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9, 14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6, 4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14, 11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3 }
                , new byte[] { 12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11, 10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8, 9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6, 4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13 }
                , new byte[] { 4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1, 13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6, 1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2, 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12 }
                , new byte[] { 13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7, 1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2, 7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8, 2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11 }
            };

            int intBox, intRow, intColumn;
            byte[] arrBytes = new byte[1]
                , arrBinary = new byte[8];
            byte a, b, c, d, e, f;

            //Create an optimized version of the s-boxes
            //this is not in the standard but much faster
            //than calculating the Row/Column index later
            for (intBox = 0; intBox < 8; intBox++)
            {
                for (a = 0; a < 2; a++)
                {
                    for (b = 0; b < 2; b++)
                    {
                        for (c = 0; c < 2; c++)
                        {
                            for (d = 0; d < 2; d++)
                            {
                                for (e = 0; e < 2; e++)
                                {
                                    for (f = 0; f < 2; f++)
                                    {
                                        intRow = a * 2 + f;
                                        intColumn = b * 8 + c * 4 + d * 2 + e;
                                        arrBytes = new byte[] { vSbox[intBox][intRow * 16 + intColumn] };

                                        Byte2Bin(arrBytes, 1, ref arrBinary);

                                        m_sBox[intBox, a, b, c, d, e, f] = new byte[4];

                                        Buffer.BlockCopy(arrBinary, 4, (byte[])m_sBox[intBox, a, b, c, d, e, f], 0, 4);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public DES(string palabraSecreta) : this()
        {
            this.Key = palabraSecreta;
        }

        #endregion

        #region [ Properties ]

        #region Key

        private string Key
        {
            set
            {
                //Do nothing if the key is buffered
                if (this._strKey == value)
                    return;

                int a;

                byte[] C = new byte[28]
                    , D = new byte[28]
                    , K = new byte[56]
                    , CD = new byte[56]
                    , Temp = new byte[2]
                    , KeyBin = new byte[64]
                    , KeySchedule = new byte[64];

                //Store a string value of the buffered key
                this._strKey = value;

                //Convert the key to a binary array
                Byte2Bin(Encoding.Default.GetBytes(value), (value.Length > 8 ? 8 : value.Length), ref KeyBin);

                //Apply the PC-2 permutation
                for (a = 0; a < 56; a++)
                {
                    KeySchedule[a] = KeyBin[this._arrPermutedChoice1[a]];
                }

                //Split keyschedule into two halves, C[] and D[]
                Buffer.BlockCopy(KeySchedule, 0, C, 0, 28);
                Buffer.BlockCopy(KeySchedule, 28, D, 0, 28);

                //Calculate the key schedule (16 subkeys)
                for (int i = 0; i < 16; i++)
                {
                    //Perform one or two cyclic left shifts on
                    //both C[i-1] and D[i-1] to get C[i] and D[i]
                    Buffer.BlockCopy(C, 0, Temp, 0, this._arrLeftShifts[i]);
                    Buffer.BlockCopy(C, this._arrLeftShifts[i], C, 0, 28 - this._arrLeftShifts[i]);
                    Buffer.BlockCopy(Temp, 0, C, 28 - this._arrLeftShifts[i], this._arrLeftShifts[i]);
                    Buffer.BlockCopy(D, 0, Temp, 0, this._arrLeftShifts[i]);
                    Buffer.BlockCopy(D, this._arrLeftShifts[i], D, 0, 28 - this._arrLeftShifts[i]);
                    Buffer.BlockCopy(Temp, 0, D, 28 - this._arrLeftShifts[i], this._arrLeftShifts[i]);

                    //Concatenate C[] and D[]
                    Buffer.BlockCopy(C, 0, CD, 0, 28);
                    Buffer.BlockCopy(D, 0, CD, 28, 28);

                    //Apply the PC-2 permutation and store
                    //the calculated subkey
                    for (a = 0; a < 48; a++)
                    {
                        this._arrKey[a, i] = CD[this._arrPermutedChoice2[a]];
                    }
                }
            }
        }

        #endregion

        #endregion

        #region [ Methods ]

        #region InitializeLeftShits

        private void InitializeLeftShits()
        {
            this._arrLeftShifts = new byte[] { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };
        }

        #endregion

        #region InitializePermuted

        private void InitializePermuted(byte[] initalValue, out byte[] permutedValue)
        {
            permutedValue = new byte[initalValue.Length];

            for (int i = 0, j = initalValue.Length; i < j; i++)
            {
                permutedValue[i] = (byte)(initalValue[i] - 1);
            }
        }

        #endregion

        #region InitializeInitialPermutation

        private void InitializeInitialPermutation()
        {
            byte[] arrInitialValue = new byte[] { 58, 50, 42, 34, 26, 18, 10, 2, 60, 52, 44, 36, 28, 20, 12, 4, 62, 54, 46, 38, 30, 22, 14, 6, 64, 56, 48, 40, 32, 24, 16, 8, 57, 49, 41, 33, 25, 17, 9, 1, 59, 51, 43, 35, 27, 19, 11, 3, 61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7 };

            this.InitializePermuted(arrInitialValue, out this._arrInitialPermutation);
        }

        #endregion

        #region InitializePermutedChoice1

        private void InitializePermutedChoice1()
        {
            byte[] arrInitialValue = new byte[] { 57, 49, 41, 33, 25, 17, 9, 1, 58, 50, 42, 34, 26, 18, 10, 2, 59, 51, 43, 35, 27, 19, 11, 3, 60, 52, 44, 36, 63, 55, 47, 39, 31, 23, 15, 7, 62, 54, 46, 38, 30, 22, 14, 6, 61, 53, 45, 37, 29, 21, 13, 5, 28, 20, 12, 4 };

            this.InitializePermuted(arrInitialValue, out this._arrPermutedChoice1);
        }

        #endregion

        #region InitializePermutedChoice2

        private void InitializePermutedChoice2()
        {
            byte[] arrInitialValue = new byte[] { 14, 17, 11, 24, 1, 5, 3, 28, 15, 6, 21, 10, 23, 19, 12, 4, 26, 8, 16, 7, 27, 20, 13, 2, 41, 52, 31, 37, 47, 55, 30, 40, 51, 45, 33, 48, 44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32 };

            this.InitializePermuted(arrInitialValue, out this._arrPermutedChoice2);
        }

        #endregion

        #region InitializeFinalPermutation

        private void InitializeFinalPermutation()
        {
            byte[] arrInitialValue = new byte[] { 40, 8, 48, 16, 56, 24, 64, 32, 39, 7, 47, 15, 55, 23, 63, 31, 38, 6, 46, 14, 54, 22, 62, 30, 37, 5, 45, 13, 53, 21, 61, 29, 36, 4, 44, 12, 52, 20, 60, 28, 35, 3, 43, 11, 51, 19, 59, 27, 34, 2, 42, 10, 50, 18, 58, 26, 33, 1, 41, 9, 49, 17, 57, 25 };

            this.InitializePermuted(arrInitialValue, out this._arrFinalPermutation);
        }

        #endregion

        #region Byte2Bin

        private void Byte2Bin(byte[] ByteArray, int ByteLen, ref byte[] BinaryArray)
        {
            //Clear the destination array, faster than
            //setting the data to zero in the loop below
            Buffer.BlockCopy(this._arrEmptyArray, 0, BinaryArray, 0, ByteLen * 8);

            //Add binary 1's where needed
            int BinLength = 0;
            byte ByteValue;

            for (int a = 0; a < ByteLen; a++)
            {
                ByteValue = ByteArray[a];

                int i = 7;

                while (ByteValue >= 1)
                {
                    BinaryArray[BinLength + i] = (byte)(ByteValue % 2);
                    ByteValue = (byte)(ByteValue / 2);
                    i--;
                }

                BinLength += 8;
            }
        }

        #endregion

        #region Bin2Byte

        private void Bin2Byte(byte[] BinaryArray, int ByteLen, ref  byte[] ByteArray)
        {
            byte ByteValue;
            long BinLength;

            //Calculate byte values
            BinLength = 0;
            for (int a = 0; a <= (ByteLen - 1); a++)
            {
                ByteValue = 0;

                for (int i = 0; i < 8; i++)
                {
                    if (BinaryArray[BinLength + i] == 1) ByteValue = (byte)(ByteValue + (Math.Pow(2, 7 - i)));
                }

                ByteArray[a] = ByteValue;

                BinLength = BinLength + 8;
            }
        }

        #endregion

        #region DecryptByte

        public void DecryptByte(ref byte[] ByteArray)
        {
            this.DecryptByte(ref ByteArray, this._strKey);
        }

        public void DecryptByte(ref byte[] ByteArray, string Key = "")
        {
            int OrigLen;
            int CipherLen;
            byte[] CurrBlock = new byte[8];
            byte[] CipherBlock = new byte[8];

            //Set the new key if provided
            if (Key.Length > 0)
                this.Key = Key;

            //Get the size of the ciphertext
            CipherLen = ByteArray.GetUpperBound(0) + 1;

            //Decrypt the data in 64-bit blocks
            for (int Offset = 0; Offset <= (CipherLen - 1); Offset += 8)
            {
                //Get the next block of ciphertext
                Buffer.BlockCopy(ByteArray, Offset, CurrBlock, 0, 8);

                //Decrypt the block
                DecryptBlock(ref CurrBlock);

                //XOR with the previous cipherblock
                for (int a = 0; a < 8; a++)
                {
                    CurrBlock[a] = (byte)(CurrBlock[a] ^ CipherBlock[a]);
                }

                //Store the current ciphertext to use
                //XOR with the next block plaintext
                Buffer.BlockCopy(ByteArray, Offset, CipherBlock, 0, 8);

                //Store the block
                Buffer.BlockCopy(CurrBlock, 0, ByteArray, Offset, 8);
            }

            //Get the size of the original array
            OrigLen = BitConverter.ToInt32(ByteArray, 8);

            //Make sure OrigLen is a reasonable value,
            //if we used the wrong key the next couple
            //of statements could be dangerous (GPF)
            if ((CipherLen - OrigLen > 19) || (CipherLen - OrigLen < 12))
            {
                throw new Exception("Incorrect size descriptor in DES decryption");
            }

            //Resize the bytearray to hold only the plaintext
            //and not the extra information added by the
            //encryption routine
            Buffer.BlockCopy(ByteArray, 12, ByteArray, 0, OrigLen);

            Array.Resize(ref ByteArray, OrigLen);
        }

        #endregion

        #region DecryptBlock

        private void DecryptBlock(ref byte[] BlockData)
        {
            int a;
            int i;
            byte[] L = new byte[32];
            byte[] R = new byte[32];
            byte[] RL = new byte[64];
            byte[] sBox = new byte[32];
            byte[] LiRi = new byte[32];
            byte[] ERxorK = new byte[48];
            byte[] BinBlock = new byte[64];

            //Convert the block into a binary array
            //(I do believe this is the best solution
            //in VB for the DES algorithm, but it is
            //still slow as xxxx)
            Byte2Bin(BlockData, 8, ref BinBlock);

            //Apply the IP permutation and split the
            //block into two halves, L[] and R[]
            for (a = 0; a < 32; a++)
            {
                L[a] = BinBlock[this._arrInitialPermutation[a]];
                R[a] = BinBlock[this._arrInitialPermutation[a + 32]];
            }

            //Apply the 16 subkeys on the block
            for (i = 15; i >= 0; i--)
            {
                //E(R[i]) xor K[i]
                ERxorK[0] = (byte)(R[31] ^ this._arrKey[0, i]);
                ERxorK[1] = (byte)(R[0] ^ this._arrKey[1, i]);
                ERxorK[2] = (byte)(R[1] ^ this._arrKey[2, i]);
                ERxorK[3] = (byte)(R[2] ^ this._arrKey[3, i]);
                ERxorK[4] = (byte)(R[3] ^ this._arrKey[4, i]);
                ERxorK[5] = (byte)(R[4] ^ this._arrKey[5, i]);
                ERxorK[6] = (byte)(R[3] ^ this._arrKey[6, i]);
                ERxorK[7] = (byte)(R[4] ^ this._arrKey[7, i]);
                ERxorK[8] = (byte)(R[5] ^ this._arrKey[8, i]);
                ERxorK[9] = (byte)(R[6] ^ this._arrKey[9, i]);
                ERxorK[10] = (byte)(R[7] ^ this._arrKey[10, i]);
                ERxorK[11] = (byte)(R[8] ^ this._arrKey[11, i]);
                ERxorK[12] = (byte)(R[7] ^ this._arrKey[12, i]);
                ERxorK[13] = (byte)(R[8] ^ this._arrKey[13, i]);
                ERxorK[14] = (byte)(R[9] ^ this._arrKey[14, i]);
                ERxorK[15] = (byte)(R[10] ^ this._arrKey[15, i]);
                ERxorK[16] = (byte)(R[11] ^ this._arrKey[16, i]);
                ERxorK[17] = (byte)(R[12] ^ this._arrKey[17, i]);
                ERxorK[18] = (byte)(R[11] ^ this._arrKey[18, i]);
                ERxorK[19] = (byte)(R[12] ^ this._arrKey[19, i]);
                ERxorK[20] = (byte)(R[13] ^ this._arrKey[20, i]);
                ERxorK[21] = (byte)(R[14] ^ this._arrKey[21, i]);
                ERxorK[22] = (byte)(R[15] ^ this._arrKey[22, i]);
                ERxorK[23] = (byte)(R[16] ^ this._arrKey[23, i]);
                ERxorK[24] = (byte)(R[15] ^ this._arrKey[24, i]);
                ERxorK[25] = (byte)(R[16] ^ this._arrKey[25, i]);
                ERxorK[26] = (byte)(R[17] ^ this._arrKey[26, i]);
                ERxorK[27] = (byte)(R[18] ^ this._arrKey[27, i]);
                ERxorK[28] = (byte)(R[19] ^ this._arrKey[28, i]);
                ERxorK[29] = (byte)(R[20] ^ this._arrKey[29, i]);
                ERxorK[30] = (byte)(R[19] ^ this._arrKey[30, i]);
                ERxorK[31] = (byte)(R[20] ^ this._arrKey[31, i]);
                ERxorK[32] = (byte)(R[21] ^ this._arrKey[32, i]);
                ERxorK[33] = (byte)(R[22] ^ this._arrKey[33, i]);
                ERxorK[34] = (byte)(R[23] ^ this._arrKey[34, i]);
                ERxorK[35] = (byte)(R[24] ^ this._arrKey[35, i]);
                ERxorK[36] = (byte)(R[23] ^ this._arrKey[36, i]);
                ERxorK[37] = (byte)(R[24] ^ this._arrKey[37, i]);
                ERxorK[38] = (byte)(R[25] ^ this._arrKey[38, i]);
                ERxorK[39] = (byte)(R[26] ^ this._arrKey[39, i]);
                ERxorK[40] = (byte)(R[27] ^ this._arrKey[40, i]);
                ERxorK[41] = (byte)(R[28] ^ this._arrKey[41, i]);
                ERxorK[42] = (byte)(R[27] ^ this._arrKey[42, i]);
                ERxorK[43] = (byte)(R[28] ^ this._arrKey[43, i]);
                ERxorK[44] = (byte)(R[29] ^ this._arrKey[44, i]);
                ERxorK[45] = (byte)(R[30] ^ this._arrKey[45, i]);
                ERxorK[46] = (byte)(R[31] ^ this._arrKey[46, i]);
                ERxorK[47] = (byte)(R[0] ^ this._arrKey[47, i]);

                //Apply the s-boxes
                for (int m = 0; m < 8; m++)
                {
                    int n = 6 * m;

                    Buffer.BlockCopy((byte[])m_sBox[m, ERxorK[n], ERxorK[n + 1], ERxorK[n + 2], ERxorK[n + 3], ERxorK[n + 4], ERxorK[n + 5]], 0, sBox, 4 * m, 4);
                }

                //L[i] xor P(R[i])
                LiRi[0] = (byte)(L[0] ^ sBox[15]);
                LiRi[1] = (byte)(L[1] ^ sBox[6]);
                LiRi[2] = (byte)(L[2] ^ sBox[19]);
                LiRi[3] = (byte)(L[3] ^ sBox[20]);
                LiRi[4] = (byte)(L[4] ^ sBox[28]);
                LiRi[5] = (byte)(L[5] ^ sBox[11]);
                LiRi[6] = (byte)(L[6] ^ sBox[27]);
                LiRi[7] = (byte)(L[7] ^ sBox[16]);
                LiRi[8] = (byte)(L[8] ^ sBox[0]);
                LiRi[9] = (byte)(L[9] ^ sBox[14]);
                LiRi[10] = (byte)(L[10] ^ sBox[22]);
                LiRi[11] = (byte)(L[11] ^ sBox[25]);
                LiRi[12] = (byte)(L[12] ^ sBox[4]);
                LiRi[13] = (byte)(L[13] ^ sBox[17]);
                LiRi[14] = (byte)(L[14] ^ sBox[30]);
                LiRi[15] = (byte)(L[15] ^ sBox[9]);
                LiRi[16] = (byte)(L[16] ^ sBox[1]);
                LiRi[17] = (byte)(L[17] ^ sBox[7]);
                LiRi[18] = (byte)(L[18] ^ sBox[23]);
                LiRi[19] = (byte)(L[19] ^ sBox[13]);
                LiRi[20] = (byte)(L[20] ^ sBox[31]);
                LiRi[21] = (byte)(L[21] ^ sBox[26]);
                LiRi[22] = (byte)(L[22] ^ sBox[2]);
                LiRi[23] = (byte)(L[23] ^ sBox[8]);
                LiRi[24] = (byte)(L[24] ^ sBox[18]);
                LiRi[25] = (byte)(L[25] ^ sBox[12]);
                LiRi[26] = (byte)(L[26] ^ sBox[29]);
                LiRi[27] = (byte)(L[27] ^ sBox[5]);
                LiRi[28] = (byte)(L[28] ^ sBox[21]);
                LiRi[29] = (byte)(L[29] ^ sBox[10]);
                LiRi[30] = (byte)(L[30] ^ sBox[3]);
                LiRi[31] = (byte)(L[31] ^ sBox[24]);

                //Prepare for next round
                Buffer.BlockCopy(R, 0, L, 0, 32);
                Buffer.BlockCopy(LiRi, 0, R, 0, 32);
            }

            //Concatenate R[]L[]
            Buffer.BlockCopy(R, 0, RL, 0, 32);
            Buffer.BlockCopy(L, 0, RL, 32, 32);

            //    'Apply the invIP permutation
            for (a = 0; a < 64; a++)
            {
                BinBlock[a] = RL[this._arrFinalPermutation[a]];
            }

            //Convert the binaries into a byte array
            Bin2Byte(BinBlock, 8, ref BlockData);
        }

        #endregion

        #endregion
    }
}