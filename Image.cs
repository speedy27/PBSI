using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace psiproject
{
    public class Image
    {
        private Pixel[,] image;
        private double[,,] dctmat = null;
        private byte[] header;
        private byte[] headerio;
        private int largeur;
        private int hauteur;
        private int offset;
        private int fichtaille;

        /// <summary>
        /// //Second constructeur initialisant une image a partir d'une matrice de pixel.
        /// </summary>
        /// <param name="image"> Créer une image en fonction d'une matrice de pixel, creer tout les header.</param>
        public Image(Pixel[,] image)
        {
            this.image = image;
            this.largeur = image.GetLength(1);
            this.hauteur = image.GetLength(0);
            fichtaille = image.Length * 3 + 54;
            this.offset = 54;

            header = new byte[14] { 66, 77, 0, 0, 0, 0, 0, 0, 0, 0, 54, 0, 0, 0 };
            headerio = new byte[40] { 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < 4; i++)
            {
                header[i + 2] = Convertir_Int_To_Endian(fichtaille)[i];
                header[i + 10] = Convertir_Int_To_Endian(offset)[i];
                headerio[i + 4] = Convertir_Int_To_Endian(largeur)[i];
                headerio[i + 8] = Convertir_Int_To_Endian(hauteur)[i];
                headerio[i + 20] = Convertir_Int_To_Endian(fichtaille - 54)[i];
            }




        }

        /// <summary>
        /// //Premier constructeur initialisant une image a partir d'un fichier
        /// </summary>
        /// <param name="file"> file designe ici le chemin du fichier qu'on va lire puis convertir en classe Image.</param>
        public Image(string file) 
        {
            byte[] fichier = File.ReadAllBytes(file);
            this.header = new byte[14];
            this.headerio = new byte[40];
            byte[] taillefich = { fichier[2], fichier[3], fichier[4], fichier[5] };
            this.fichtaille = Convertir_Endian_To_Int(taillefich);


            for (int i = 0; i < 14; i++)
                header[i] = fichier[i];

            for (int i = 14; i < 54; i++)
                headerio[i - 14] = fichier[i];


            byte[] blargeur = { fichier[18], fichier[19], fichier[20], fichier[21] };
            byte[] bhauteur = { fichier[22], fichier[23], fichier[24], fichier[25] };
            byte[] boff = { fichier[10], fichier[11], fichier[12], fichier[13] };

            this.offset = Convertir_Endian_To_Int(boff);
            this.hauteur = Convertir_Endian_To_Int(bhauteur);
            this.largeur = Convertir_Endian_To_Int(blargeur);
            image = new Pixel[hauteur, largeur];
            int k = 54;
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    image[i, j] = new Pixel(fichier[k], fichier[k + 1], fichier[k + 2]);
                    k += 3;

                }
            }
        }


        public Pixel[,] Images//Propriété pixel.
        {
            get { return image; }
            set { image = value; }
        }

        public double[,,] Dctmat//Propriété pixel.
        {
            get { return dctmat; }
            set { dctmat = value; }
        }

        /// <summary>
        /// // Ecris dans un fichier une image.
        /// </summary>
        /// <param name="file" > file désigne le chemin du fichier de l'image bmp. et ecris une image dans ce chemin.</param>
        public void From_Image_To_File(string file) 
        {

            for (int i = 0; i < 4; i++)
            {
                header[i + 2] = Convertir_Int_To_Endian(fichtaille)[i];
                header[i + 10] = Convertir_Int_To_Endian(offset)[i];
                headerio[i + 4] = Convertir_Int_To_Endian(largeur)[i];
                headerio[i + 8] = Convertir_Int_To_Endian(hauteur)[i];
                headerio[i + 20] = Convertir_Int_To_Endian(fichtaille - 54)[i];
            }
            byte[] rep = isconform();

            File.WriteAllBytes(file, rep);
        }

        /// <summary>
        /// Vérifie la conformité d'un fichier bmp par raport au nombre d'octer qui doit etre un multiple de 4.
        /// </summary>
        /// <returns>retourne un tableau de bytes. Qui contient toute la matrice.</returns>
        public byte[] isconform()
        {
            byte[] rep = new byte[54];


            for (int i = 0; i < 14; i++)
                rep[i] = header[i];

            for (int i = 14; i < 54; i++)
                rep[i] = headerio[i - 14];

            int k = 0;
            int nbbyte = largeur * 3;
            while (nbbyte % 4 != 0)
            {
                nbbyte++;
            }
            nbbyte = nbbyte - largeur * 3;
            byte[] mat = new byte[image.Length * 3 + nbbyte * hauteur];

            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    mat[k] = image[i, j].Rouge;
                    mat[k + 1] = image[i, j].Vert;
                    mat[k + 2] = image[i, j].Bleu;
                    k += 3;
                }
                for (int l = 0; l < nbbyte; l++)
                {
                    mat[k] = 0;
                    k++;
                }




            }
            rep = Concatenerbyte(rep, mat);
            return rep;
        }

        /// <summary>
        /// // Concatene deux tableaux de bytes.
        /// </summary>
        /// <param name="b1"> Premier tableau</param>
        /// <param name="b2"> Deuxième tableau</param>
        /// <returns></returns>
        public byte[] Concatenerbyte(byte[] b1, byte[] b2) 
        {
            byte[] newbyte = new byte[b1.Length + b2.Length];
            int i = 0;
            foreach (byte b in b1)
            {
                newbyte[i] = b;
                i++;
            }
            foreach (byte b in b2)
            {
                newbyte[i] = b;
                i++;
            }
            return newbyte;
        }

        /// <summary>
        /// Affiche l'image tout simplement, affiche chaque octet de la matrice d'image.
        /// </summary>
        /// <returns></returns>
        public string toString() //affichage matrice pixel
        {
            string rep = "";
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    rep += this.image[i, j].toString();
                }
                rep += "\n";
            }
            return rep;
        }

        /// <summary>
        /// // Methode de base conversion en little indian.
        /// </summary>
        /// <param name="tab"> le tableau en little endian en octet a convertir.</param>
        /// <returns></returns>
        public int Convertir_Endian_To_Int(byte[] tab) 
        {
            return tab[0] + tab[1]*256 + tab[2]* 256* 256 + tab[3]* 256* 256* 256;
        }

        /// <summary>
        /// //Méthode de base conversion en int , a la main possible.
        /// </summary>
        /// <param name="val"> Valeur en int à convertir</param>
        /// <returns></returns>
        public byte[] Convertir_Int_To_Endian(int val) 
        {
            byte b1 = 0,b2=0,b3=0;
            while(256*256*256 <= val)
            {
                b1++;
                val -= 256 * 256 * 256;
            }
            while (256 * 256  <= val)
            {
                b2++;
                val -= 256 * 256 ;
            }
            while (256  <= val)
            {
                b3++; ;
                val -= 256 ;
            }
            return new byte[4] { (byte)val,b3,b2,b1};
        }

        /// <summary>
        ///  Méthode opérant la rotation d'une image en fonction d'un angle.
        /// </summary>
        /// <param name="angle">L'angle de rotation qu'on va effectuer.</param>
        public void rotalpha(double angle) 
        {
            double rad = (angle * Math.PI) / 180;
            int maxLargeur = (int)Math.Ceiling(Math.Abs(largeur * Math.Sqrt(2) / 2) + Math.Abs(hauteur * Math.Sqrt(2) / 2));
            int maxHauteur = (int)Math.Ceiling(Math.Abs(largeur * Math.Sqrt(2) / 2) + Math.Abs(hauteur * Math.Sqrt(2) / 2));
            Pixel[,] rep = new Pixel[maxHauteur, maxLargeur];

            int x = 0, y = 0;
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    x = (int)Math.Round((j - largeur / 2) * Math.Cos(rad) + (hauteur / 2 - i) * Math.Sin(rad) + maxHauteur / 2);
                    y = (int)Math.Round((j - largeur / 2) * Math.Sin(rad) - (hauteur / 2 - i) * Math.Cos(rad) + maxLargeur / 2);
                    if (x >= 0 && x < maxHauteur && y >= 0 && y < maxLargeur)
                    {
                        rep[y, x] = image[i, j];
                    }
                }
            }


            hauteur = maxHauteur;
            largeur = maxLargeur;
            fichtaille = 3 * (maxLargeur * maxHauteur) + 54;

            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    if (rep[i, j] == null)
                    {
                        rep[i, j] = Pixelvoisin(rep, i, j);
                        rep[i, j] = Pixelvoisin(rep, i, j);
                    }
                }


                image = rep;

            }

        }

        /// <summary>
        /// // Méthode permettant de remplir les trous noir dans une image apres une rotation.
        /// </summary>
        /// <param name="pixels"> Matrice de pixel dans laqulle on va opérer.</param>
        /// <param name="i">Position de notre pixel en i.</param>
        /// <param name="j">Position d notre pixel en j.</param>
        /// <returns>Retourne rien car la rotation se fait dans la matrice de la classe Image.</returns>
        public Pixel Pixelvoisin(Pixel[,] pixels, int i, int j) 
        {
            List<Pixel> rep = new List<Pixel>();
            int m = 0;
            for (int k = 0; k <= 1; k++)
            {
                for (int l = 0; l <= 1; l++)
                {
                    if (i > 1 && i < pixels.GetLength(0) - 1 && j > 1 && j < pixels.GetLength(1) - 1)
                    {
                        if (pixels[i + k, j + l] != null)
                        {
                            rep.Add(pixels[i + k, j + l]);
                        }
                        else { rep.Add(new Pixel(0, 0, 0)); }
                        if (pixels[i - k, j + l] != null)
                        {
                            rep.Add(pixels[i - k, j + l]);
                        }
                        else { rep.Add(new Pixel(0, 0, 0)); }
                        if (pixels[i + k, j - l] != null)
                        {
                            rep.Add(pixels[i + k, j - l]);
                        }
                        else { rep.Add(new Pixel(0, 0, 0)); }
                        if (pixels[i - k, j - l] != null)
                        {
                            rep.Add(pixels[i - k, j - l]);
                        }
                        else { rep.Add(new Pixel(0, 0, 0)); }
                    }
                }
            }
            Random r = new Random();
            Pixel pix = new Pixel(0, 0, 0);
            int moyenner = 0;
            int moyennev = 0;
            int moyenneb = 0;
            if (rep.Count != 0)
            {
                foreach (Pixel p in rep)
                {
                    moyenner += p.Rouge;
                    moyennev += p.Vert;
                    moyenneb += p.Bleu;
                }
                moyenner = moyenner / rep.Count;
                moyennev = moyennev / rep.Count;
                moyenneb = moyenneb / rep.Count;
                pix = new Pixel((byte)moyenner, (byte)moyennev, (byte)moyenneb);
            }



            return pix;
        }

        /// <summary>
        /// // applique le filtre a une image.
        /// </summary>
        /// <param name="filtre"> Matrice de convolution qui va permettre le filtrage.</param>
        public void appfiltres(int[,] filtre) 
        {
            int filtrehauteur = filtre.GetLength(0);
            int filtrelargeur = filtre.GetLength(1);

            Pixel[,] result = new Pixel[hauteur, largeur];

            for (int y = 0; y < hauteur; y++)
            {
                for (int x = 0; x < largeur; x++)
                {
                    int sumR = 0;
                    int sumG = 0;
                    int sumB = 0;

                    for (int fy = 0; fy < filtrehauteur; fy++)
                    {
                        for (int fx = 0; fx < filtrelargeur; fx++)
                        {
                            int imageX = x - filtrelargeur / 2 + fx;
                            int imageY = y - filtrehauteur / 2 + fy;

                            if (imageX >= 0 && imageX < largeur && imageY >= 0 && imageY < hauteur)
                            {
                                Pixel Pixel = image[imageY, imageX];
                                int filtreVal = filtre[fy, fx];

                                sumR += Pixel.Rouge * filtreVal;
                                sumG += Pixel.Vert * filtreVal;
                                sumB += Pixel.Bleu * filtreVal;
                            }
                        }
                    }

                    byte newR = (byte)Math.Max(0, Math.Min(sumR, 255));
                    byte newG = (byte)Math.Max(0, Math.Min(sumG, 255));
                    byte newB = (byte)Math.Max(0, Math.Min(sumB, 255));
                    result[y, x] = new Pixel(newR, newG, newB);
                }
            }

            image = result;
        }

        /// <summary>
        /// //fonction de filtrages, 3 filtres sont proposé, le flou, detection de contour ainsi qu'un autre stylé.
        /// </summary>
        /// <return> rien car applique le filtre dans la amtrice de la classe.</return>
        public void filtre()
        {
            int[,] filtre =
            {
                  { 0,  0,  -1,  0,  0 },
                  { 0,  0,  -1,  0,  0 },
                  { 0,  0,  2,  0,  0 },
                  { 0,  0,  0,  0,  0 },
                  { 0,  0,  0,  0,  0 }
             };
            int[,] filtre1 =
            {
                  { 0,  1,  0},
                  { 1,  1,  1},
                  { 0,  1,  0},
             };
            int[,] filtre2 =
            {
                  { -1,  -1, -1},
                  { -1,  9, -1},
                  { -1,  -1,  1}
             };

            appfiltres(filtre);
        }

        /// <summary>
        /// // produit matriciel, inutile dans notre contexte.
        /// </summary>
        /// <param name="A">Matrice A</param>
        /// <param name="B"> Matrice B</param>
        /// <returns>de meme le filtrr est appliqué grace à appfiltre.</returns>
        public double[,] Prodmat(double[,] A, double[,] B) 
        {
            double[,] C = null;
            if (A.GetLength(1) != B.GetLength(0)) { }
            else
            {
                C = new double[A.GetLength(0), B.GetLength(1)];

                for (int i = 0; i < A.GetLength(0); i++)
                {
                    for (int j = 0; j < B.GetLength(1); j++)
                    {
                        C[i, j] = 0;
                        for (int k = 0; k < A.GetLength(1); k++)
                        {
                            C[i, j] += A[i, k] * B[k, j];
                        }
                    }
                }
            }
            return C;
        }

        /// <summary>
        /// // agrandi une image.
        /// </summary>
        /// <param name="c">Facteur d'agrandissement</param>
        public void Agrandissement(int c) 
        {
            hauteur = hauteur * c;
            largeur = largeur * c;
            Pixel[,] rep = new Pixel[hauteur, largeur];

            for (int i = 0; i < rep.GetLength(0); i++)
            {
                for (int j = 0; j < rep.GetLength(1); j++)
                {
                    rep[i, j] = image[i / c, j / c];
                }
            }
            image = rep;
        } // a continuer

        /// <summary>
        /// // Conversion d'un byte en tableau de bits.
        /// </summary>
        /// <param name="b">Byte à convertir.</param>
        /// <returns>le changement est opéré dans l'attribut de la classe image: image.</returns>
        public byte[] ConvertBytetoBASE2(byte b)
        {
            int i = 7, k = 0; byte[] rep = new byte[8];
            while (i >= 0)
            {
                if (Math.Pow(2, i) <= (int)b)
                {
                    rep[k] = 1;
                    b -= (byte)Math.Pow(2, i);
                }
                else
                {
                    rep[k] = 0;
                }
                i--;
                k++;
            }
            return rep;
        }

        /// <summary>
        /// //Converti un tableau de bits en un byte.
        /// </summary>
        /// <param name="tab"> Tableau de bits a convertir.</param>
        /// <returns>tableau de bits.</returns>
        public byte ConverttoBASE2TOBYTE(byte[] tab) 
        {
            byte b = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                b += (byte)((int)tab[i] * (int)Math.Pow(2, tab.Length - 1 - i));
            }
            return b;
        }

        /// <summary>
        /// // Cache les bytes de deux Pixels pour la cacher dans un autre Pixel.
        /// </summary>
        /// <param name="p1"> pixel p1.</param>
        /// <param name="p2"> pixel p2.</param>
        /// <returns> on va retourner un pixel qui va contenir sois p1 sois p2 caché dans l'autre pixel.</returns>
        public Pixel cacherbyte(Pixel p1, Pixel p2)
        {
            byte[] r1 = ConvertBytetoBASE2(p1.Rouge);
            byte[] r2 = ConvertBytetoBASE2(p2.Rouge);
            byte[] b1 = ConvertBytetoBASE2(p1.Bleu);
            byte[] b2 = ConvertBytetoBASE2(p2.Bleu);
            byte[] v1 = ConvertBytetoBASE2(p1.Vert);
            byte[] v2 = ConvertBytetoBASE2(p2.Vert);

            byte[] fr = { r1[0], r1[1], r1[2], r1[3], r2[0], r2[1], r2[2], r2[3] };
            byte[] fb = { b1[0], b1[1], b1[2], b1[3], b2[0], b2[1], b2[2], b2[3] };
            byte[] fv = { v1[0], v1[1], v1[2], v1[3], v2[0], v2[1], v2[2], v2[3] };

            byte R = ConverttoBASE2TOBYTE(fr);
            byte V = ConverttoBASE2TOBYTE(fv);
            byte B = ConverttoBASE2TOBYTE(fb);
            return new Pixel(R, V, B);
        }

        /// <summary>
        ///  // REtrouve les bytes de deux images cachées dans une seule image.
        /// </summary>
        /// <param name="p"> on part du principe que ce pixel quon va prendre en parametre contient deux images.</param>
        /// <returns>on va retourner deux pixel dans un tableau de pixel chacun correspond à une image différente.</returns>
        public Pixel[] retrouverbyte(Pixel p)
        {
            byte[] r = ConvertBytetoBASE2(p.Rouge);
            byte[] b = ConvertBytetoBASE2(p.Bleu);
            byte[] v = ConvertBytetoBASE2(p.Vert);

            byte[] fr1 = { r[0], r[1], r[2], r[3], 0, 0, 0, 0 };
            byte[] fr2 = { r[4], r[5], r[6], r[7], 0, 0, 0, 0 };
            byte[] fv1 = { v[0], v[1], v[2], v[3], 0, 0, 0, 0 };
            byte[] fv2 = { v[4], v[5], v[6], v[7], 0, 0, 0, 0 };
            byte[] fb1 = { b[0], b[1], b[2], b[3], 0, 0, 0, 0 };
            byte[] fb2 = { b[4], b[5], b[6], b[7], 0, 0, 0, 0 };

            byte R1 = ConverttoBASE2TOBYTE(fr1);
            byte V1 = ConverttoBASE2TOBYTE(fv1);
            byte B1 = ConverttoBASE2TOBYTE(fb1);
            byte R2 = ConverttoBASE2TOBYTE(fr2);
            byte V2 = ConverttoBASE2TOBYTE(fv2);
            byte B2 = ConverttoBASE2TOBYTE(fb2);
            Pixel[] rep = new Pixel[2] { new Pixel(R1, V1, B1), new Pixel(R2, V2, B2) };
            return rep;
        }

        /// <summary>
        /// //Retrouve une image dans une autre, et retounr 2 images. et applique seulement les methode juste au dessus sur tout les pixel de la matrice.
        /// </summary>
        /// <returns> return deux images car en effet on part d'une image pour en extraire deux différente.</returns>
        public Image[] retrouverimage()
        {
            Pixel[,] image1 = new Pixel[hauteur, largeur];
            Pixel[,] image2 = new Pixel[hauteur, largeur];
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    image1[i, j] = this.retrouverbyte(this.image[i, j])[0];
                    image2[i, j] = this.retrouverbyte(this.image[i, j])[1];
                }
            }
            Image[] rep = new Image[2] { new Image(image1), new Image(image2) };
            return rep;
        }

        /// <summary>
        /// //Cache une image dans une image n utilisant les differetnes autres images.
        /// </summary>
        /// <param name="im"> Prend l'image en parametre et la cache dans l'image actuelle, les talles sont prises en compte.</param>
        public void cacherimage(Image im)
        {
            Pixel[,] rep = null;
            if (im.largeur <= largeur && im.hauteur <= hauteur)
            {
                rep = new Pixel[hauteur, largeur];

                for (int i = 0; i < this.hauteur; i++)
                {
                    for (int j = 0; j < this.largeur; j++)
                    {
                        if (j >= im.largeur || i >= im.hauteur)
                        {
                            rep[i, j] = this.image[i, j];
                        }
                        else
                        {
                            rep[i, j] = cacherbyte(this.image[i, j], im.image[i, j]);
                        }
                    }
                }
            }
            if (im.largeur > largeur && im.hauteur > hauteur)
            {
                rep = new Pixel[im.hauteur, im.largeur];


                for (int i = 0; i < im.hauteur; i++)
                {
                    for (int j = 0; j < im.largeur; j++)
                    {
                        if (j >= this.largeur || i >= this.hauteur)
                        {
                            rep[i, j] = im.image[i, j];
                        }
                        else
                        {
                            rep[i, j] = cacherbyte(im.image[i, j], this.image[i, j]);
                        }
                    }
                }
                largeur = im.largeur;
                hauteur = im.hauteur;
                fichtaille = 3 * (hauteur * largeur) + 54;
            }
            this.image = rep;
        }

        /// <summary>
        /// //Sous echantillonages 4,2,2 avec colone et lignes alternées par raport a la luminance.
        /// </summary>
        public void sous_echantillonage() 
        {
            int k = 1;
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < hauteur; j++)
                {
                    image[i, j].transformationdeculeurs();
                    if (k%2 == 0)
                    {
                        image[i, j].Bleu = 0;
                        image[i, j].Vert = 0;
                        k++;
                    }
                    else
                    {
                        k++;
                    }
                }
                k++;
            }
        }

        /// <summary>
        /// // Agrandi le facteur pour decouper la matrice en parties de 8 pixels.
        /// </summary>
        /// <param name="x"> permet d'avoir une image de longueur et de largeur divisible par 8, pour rendre possible une division d'une image en bloc de 8*8 pixels.</param>
        public void agrandissementfacteur(int x) 
        {
            int h = hauteur, l = largeur;
            while (hauteur % x != 0 || largeur % x != 0)
            {
                if (hauteur % x == 0)
                {
                    largeur++;
                }
                else if (largeur % x == 0)
                {
                    hauteur++;
                }
                else
                {
                    hauteur++;
                    largeur++;
                }
            }
            Pixel[,] rep = new Pixel[hauteur, largeur];
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < hauteur; j++)
                {
                    if (i >= h || j >= l)
                    {
                        rep[i, j] = new Pixel(0, 0, 0);
                    }
                    else
                    {
                        rep[i, j] = image[i, j];
                    }
                }
            }
            image = rep;
        }

        /// <summary>
        /// Découpe une image en bloc de 8*8 pixels
        /// </summary>
        /// <returns>Retourne une matrice d'images chacune composé de 8*8 pixels</returns>
        public Image[,] decoupage()//Decoupages en plusieurs image utilisant le second contructeurs, pour me facilier la taches
        {
            agrandissementfacteur(8);
            Image[,] rep = new Image[hauteur / 8, largeur / 8];
            Pixel[,] rep2 = new Pixel[8, 8];
            for (int i = 0; i < rep.GetLength(0); i++)
            {
                for (int j = 0; j < rep.GetLength(1); j++)
                {
                    for (int p = 0 + 8 * i; p < 8 + 8 * i; p++)
                    {
                        for (int k = 0 + j * 8; k < 8 + j * 8; k++)
                        {
                            rep2[p - i * 8, k - j * 8] = image[p, k];
                        }
                    }
                    rep[i, j] = new Image(rep2);
                }
            }
            return rep;

        }

        /// <summary>
        /// //Transformé DCT pour la compression jpeg
        /// </summary>
        /// <param name="mat">Prend une matrice de pixel en parametre et retourne les fréquence DCT en double. </param>
        public void transforméDCT(Pixel[,] mat) 
        {
            double[,,] rep = new double[8, 8, 3];
            double cons = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (i == 0 && j == 0) { cons = 1.0 / Math.Sqrt(2); } else { cons = 1; }
                    rep[i, j, 0] = Math.Round(1.0 / 4.0 * cons * resultat_somme(mat, i, j)[0]);
                    rep[i, j, 1] = Math.Round(1.0 / 4.0 * cons * resultat_somme(mat, i, j)[1]);
                    rep[i, j, 2] = Math.Round(1.0 / 4.0 * cons * resultat_somme(mat, i, j)[2]);
                }
            }
            dctmat = rep;

        }
        /// <summary>
        /// // Permet de calculer la double somme dans la formule de la transformée DCT, Compréssion JPEG
        /// </summary>
        /// <param name="mat1">Matrice de la tranformé dct prise pour permettre de calculer certains parametre de cette matrice.</param>
        /// <param name="k"> Correspons ici a k et à l et en réalité represente els coordonnées i,j de la matrice dct.</param>
        /// <param name="l"> de meme pour ce parametre.</param>
        /// <returns>permet de calculer ce qu'il ya dans la matrice dct aux positions i,j : k,l ici </returns>
        public double[] resultat_somme(Pixel[,] mat1, int k, int l)
        {
            double[] rep = new double[3];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    rep[0] += mat1[i, j].Rouge * Math.Cos((2 * i + 1) * k * Math.PI / 16.0) * Math.Cos((2 * j + 1) * l * Math.PI / 16.0);
                    rep[1] += mat1[i, j].Vert * Math.Cos((2 * i + 1) * k * Math.PI / 16.0) * Math.Cos((2 * j + 1) * l * Math.PI / 16.0);
                    rep[2] += mat1[i, j].Bleu * Math.Cos((2 * i + 1) * k * Math.PI / 16.0) * Math.Cos((2 * j + 1) * l * Math.PI / 16.0);
                }
            }
            return rep;
        }
        /// <summary>
        /// // Etape de compression JPEG, quantifie les valeurs de la amtrice.
        /// Divise notre matrice dct par des coefficient permettant de supprimer les valeurs insignifiantes.
        /// </summary>
        public void Quantification() 
        {
            int[,] quantificationmatrice =
            {
                { 16, 11, 10, 16, 24, 40, 51, 61 },
                { 12, 12, 14, 19, 26, 58, 60, 55 },
                { 14, 13, 16, 24, 40, 57, 69, 56 },
                { 14, 17, 22, 29, 51, 87, 80, 62 },
                { 18, 22, 37, 56, 68, 109, 103, 77 },
                { 24, 35, 55, 64, 81, 104, 113, 92 },
                { 49, 64, 78, 87, 103, 121, 120, 101 },
                { 72, 92, 95, 98, 112, 100, 103, 99 }
            };

            for (int i = 0; i < dctmat.GetLength(0); i++)
            {
                for (int j = 0; j < dctmat.GetLength(0); j++)
                {
                    for(int k = 0; k < 3; k++)
                    {
                        dctmat[i, j, k] = Math.Round(dctmat[i, j, k] / quantificationmatrice[i, j]);
                        if (dctmat[i,j,k] == -0) { dctmat[i, j, k] = 0; }
                    }
                }
            }

        }

        /// <summary>
        /// //galere celui la, Methode qui prend toutes les valeurs d'une matrice en diagonale. Pour la compression JPEG
        /// </summary>
        /// <returns>Ecris le resultat dans dctmat donc pas besoin de retourner le resultat.</returns>
        public string[] toStringspecial() 
        {

            double[,,] mat = dctmat;
            string chaine1 = "" + mat[0, 0, 0] + ";";
            string chaine2 = "" + mat[0, 0, 1] + ";";
            string chaine3 = "" + mat[0, 0, 2] + ";";
            int l = 1;
            int c = 1;
            for (int i = 0; i < 13; i++)
            {
                if (i <= 6)
                {
                    if (i % 2 == 0)
                    {
                        chaine1 += mat[0, l, 0] + ";";
                        chaine2 += mat[0, l, 1] + ";";
                        chaine3 += mat[0, l, 2] + ";";
                        for (int k = 1; k <= c; k++)
                        {
                            chaine1 += mat[k, l - k, 0] + ";";
                            chaine2 += mat[k, l - k, 1] + ";";
                            chaine3 += mat[k, l - k, 2] + ";";
                        }

                        c++; l++;
                        if (i == 6) { c = c - 2; l = 1; }
                    }
                    else
                    {

                        chaine1 += mat[l, 0, 0] + ";";
                        chaine2 += mat[l, 0, 1] + ";";
                        chaine3 += mat[l, 0, 2] + ";";
                        for (int k = 1; k <= c; k++)
                        {
                            chaine1 += mat[l - k, k, 0] + ";";
                            chaine2 += mat[l - k, k, 1] + ";";
                            chaine3 += mat[l - k, k, 2] + ";";
                        }
                        l++; c++;
                    }
                }
                else
                {
                    if (i % 2 == 0)
                    {
                        chaine1 += mat[l, 7, 0] + ";";
                        chaine2 += mat[l, 7, 1] + ";";
                        chaine3 += mat[l, 7, 2] + ";";
                        for (int k = 1; k <= c; k++)
                        {
                            chaine1 += mat[l + k, 7 - k, 0] + ";";
                            chaine2 += mat[l + k, 7 - k, 1] + ";";
                            chaine3 += mat[l + k, 7 - k, 2] + ";";
                        }
                        c--; l++;
                    }
                    else
                    {

                        chaine1 += mat[7, l, 0] + ";";
                        chaine2 += mat[7, l, 1] + ";";
                        chaine3 += mat[7, l, 2] + ";";
                        for (int k = 1; k <= c; k++)
                        {
                            chaine1 += mat[7 - k, l + k, 0] + ";";
                            chaine2 += mat[7 - k, l + k, 1] + ";";
                            chaine3 += mat[7 - k, l + k, 2] + ";";
                        }
                        l++; c--;
                    }
                }
            }
            string[] rep = new string[3] { chaine1 + mat[7, 7, 0] + ";", chaine2 + mat[7, 7, 1] + ";", chaine3 + mat[7, 7, 2] + ";" };
            return rep;

        }

        /// <summary>
        /// // Méthode qui prend en parametre une chaine de caractere, selon mon format et la compresse facon rle.
        /// </summary>
        /// <param name="chaine">La chaine de charactere qu'on va compresser.</param>
        /// <returns>REturn un chaine qui sera la compression en format string.</returns>
        public string RLE(string chaine) 
        {
            string rep = "";
            int k = 1;
            int l = 0;
            for (int i = 0; i < chaine.Length; i++)
            {
                if (chaine[i] != ';')
                {
                    if (chaine[i] != '0')
                    {
                        while (i < chaine.Length && chaine[i] != ';')
                        {
                            rep += chaine[i];
                            i++;
                        }
                        rep += ';';
                    }
                    else
                    {
                        int j = 2;
                        while (i + j < chaine.Length && chaine[i + j] == '0')
                        {
                            k++;
                            j += 2;
                        }
                        rep += k + ":0;";
                        i += j - 2;
                        k = 1;
                    }
                }
            }

            return rep;
        }

        /// <summary>
        /// Pas tres improtant, créer une image avecun effet spéciale.
        /// </summary>
        public void innovationContour()
        {
            int k = 0;
            while (k < hauteur && k < largeur) 
            { 
                if (k % 2 == 0)
                {
                    for (int j = k; j < largeur - k; j++)
                    {

                        image[k, j] = new Pixel(0, 0, 0);
                    }
                    for (int j = k; j < largeur - k; j++)
                    {
                        image[k, j] = new Pixel(0, 0, 0);
                    }
                    for (int i = k; i < hauteur - k; i++)
                    {
                        image[i, k] = new Pixel(0, 0, 0);
                    }
                    for (int i = k; i < hauteur - k; i++)
                    {
                        image[i, k] = new Pixel(0, 0, 0);
                    }
                    k++;
                }
                else
                {
                    byte b = (byte)k;
                    for (int j = k; j < largeur - k; j++)
                    {

                        image[k, j] = new Pixel(b, b,b);
                    }
                    for (int j = k; j < largeur - k; j++)
                    {
                        image[k, j] = new Pixel(b, b, b);
                    }
                    for (int i = k; i < hauteur - k; i++)
                    {
                        image[i, k] = new Pixel(b, b, b);
                    }
                    for (int i = k; i < hauteur - k; i++)
                    {
                        image[i, k] = new Pixel(b, b, b);
                    }
                    k++;
                }
            }

        }





    }
}
