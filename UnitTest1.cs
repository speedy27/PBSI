using static System.Net.Mime.MediaTypeNames;
using psiproject;
using System.Collections;

namespace TestProjectPSI
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Convertir_Endian_To_Int()
        {

            psiproject.Image i = new psiproject.Image("Test.bmp");
            byte[] rep = { 4, 21, 1, 0 };
            Assert.AreEqual(i.Convertir_Endian_To_Int(rep), 70916);
        }
        [TestMethod]
        public void Testinttoendian()
        {
            psiproject.Image i = new psiproject.Image("Test.bmp");
            byte[] rep = new byte[4] { 4, 21, 1, 0 };

            byte[] b = i.Convertir_Int_To_Endian(70916);
            Assert.AreEqual(b[0], rep[0]);
            Assert.AreEqual(b[1], rep[1]);
            Assert.AreEqual(b[2], rep[2]);
            Assert.AreEqual(b[3], rep[3]);
        }
        [TestMethod]
        public void TestHuffman()
        {
            Huffman Hu = new Huffman("A;B;C;D;E;F");
            BitArray test = Hu.Encode(new string[1] { "A" });
            Assert.AreEqual(test[0], true);
            Assert.AreEqual(test[1], false);
            Assert.AreEqual(test[2], false);
        }
        [TestMethod]
        public void TesttoStringSpéciale()
        {
            Pixel t = new Pixel(0, 0, 0);
            Pixel[,] test = new Pixel[1, 1] { { t } };
            psiproject.Image i = new psiproject.Image(test);

            Assert.AreEqual(i.toString(), "" + t.Rouge + " " + t.Vert + " " + t.Bleu + "\n");



        }

        [TestMethod]
        public void TestNoeud()
        {
            Huffman Hu = new Huffman("A;B;C;D;E;F");
            Noeud n = new Noeud("D", 1);

            Assert.IsTrue(n.Egale(Hu.Root.Droit.Droit.Droit));
        }
    }
}