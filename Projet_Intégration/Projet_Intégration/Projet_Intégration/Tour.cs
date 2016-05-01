using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace AtelierXNA
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Tour : Microsoft.Xna.Framework.GameComponent
    {
        //const Vector3 POS_BLANC = null;
        const string WHITE = "White";
        const string BLACK = "Black";
        float TempsÉcouléDepuisMAJ { get; set; }
        protected List<Cases> ListeDesCases { get; set; }
        protected List<Pieces> ListeDesPièces { get; set; }
        protected bool Action { get; set; }
        public string Couleur { get; set; }
        public float NbSortiesBlanc { get; set; }
        public float NbSortiesNoir { get; set; }
        CaméraSubjective CaméraJeu { get; set; }
        InputManager GestionInput { get; set; }
        Cases CaseA { get; set; }
        Cases CaseB { get; set; }
        Pieces PièceA { get; set; }
        Pieces PièceB { get; set; }
        protected int Compteur { get; set; }
        private int NbPiece { get; set; }
        public bool PartieTerminée { get; set; }

        public Tour(Game game, string couleur, List<Cases> listeCases, List<Pieces> listePièce, float nbSortiesBlanc, float nbSortiesNoir)
            : base(game)
        {
            Couleur = couleur;
            ListeDesCases = listeCases;
            ListeDesPièces = listePièce;
            NbSortiesBlanc = nbSortiesBlanc;
            NbSortiesNoir = nbSortiesNoir;
            NbPiece = 32;
        }


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            TempsÉcouléDepuisMAJ = 0;
            Compteur = 0;
            PartieTerminée = false;
            CaméraJeu = Game.Services.GetService(typeof(Caméra)) as CaméraSubjective;
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {

            if (Compteur % 2 == 0)
            {

                Couleur = WHITE;
            }
            else
            {
                Couleur = BLACK;
            }

            GérerDéplacement();
            if (EstMat())
            {
                this.Game.Exit();
            }
            //if (!VerificationMat())
            //{
            //    this.Game.Exit();
            //}

            base.Update(gameTime);
        }

        //public bool Mat();

        public bool EstMat()
        {
            bool t = true;

            foreach (Pieces l in ListeDesPièces.FindAll(x => x.Couleur != Couleur))
            {

                Pieces laPiece = null;
                Vector3 posPiece = Vector3.Zero;
                Vector3 posIni = l.Position;
                foreach (Cases r in ListeDesCases)
                {


                    if (l.LogiqueDéplacement(new Vector2((r.Centre.Z - l.Position.Z), (r.Centre.X - l.Position.X))) && NeSautePas(r.Centre, l.Position))
                    {

                        l.Deplacer(r.Centre);
                        foreach (Pieces u in ListeDesPièces.FindAll(x => x.Couleur != l.Couleur))
                        {
                            if (u.Position == r.Centre)
                            {
                                laPiece = u;
                                posPiece = u.Position;
                                laPiece.Sortir(1000, 1000);


                            }
                        }

                        l.NbDéplacement--;


                        foreach (Pieces q in ListeDesPièces.FindAll(x => x.Couleur == l.Couleur))
                        {
                            if (l != q && q.Position == l.Position)
                            {
                                l.Deplacer(posIni);
                                l.NbDéplacement--;
                            }


                        }
                        if (l.Nom == "/pawn")
                        {
                            if (laPiece == null)
                            {
                                if ((r.Centre.X - posIni.X != 0) && (r.Centre.Z - posIni.Z != 0))
                                {
                                    l.Deplacer(posIni);
                                    l.NbDéplacement--;
                                }
                            }





                        }

                        if (l.Nom == "/king" && Math.Abs(r.Centre.Z - posIni.Z) > 2)
                        {
                            l.Deplacer(posIni);
                            l.NbDéplacement--;

                        }
                        if (!EstEnEchec(ListeDesCases, ListeDesPièces, l.Couleur))
                        {

                            t = false;

                        }

                        l.Deplacer(posIni);
                        if (laPiece != null)
                        {
                            laPiece.Deplacer(posPiece);
                        }
                        l.NbDéplacement--;
                        if (l.NbDéplacement == 0)
                        {
                            l.EstPremierMove = true;
                        }




                    }

                }

            }

            return t;


            if (t == false)
            {
                //  this.Game.Exit();
            }
            return t;

        }

        private bool EstEnEchec(List<Cases> listeCases, List<Pieces> listePieces, string CouleurDuRoi)
        {
            bool condition = false;
            Pieces leRoi = ListeDesPièces.Find(x => x.Nom == "/king" && x.Couleur == CouleurDuRoi);


            foreach (Pieces a in ListeDesPièces)
            {
                Vector3 déplacement = new Vector3((leRoi.Position.X - a.Position.X), 0f, (leRoi.Position.Z - a.Position.Z));
                if (a.LogiqueDéplacement(new Vector2((leRoi.Position.X - a.Position.X), (leRoi.Position.Z - a.Position.Z))) && (a.Couleur != leRoi.Couleur) && NeSautePas(leRoi.Position, a.Position))
                {

                    condition = true;
                    if (a.Nom == "/pawn")
                    {
                        condition = a.EstValidePion(déplacement);
                    }
                }



            }
            return condition;

            //return false;
        }
        private bool NeSautePas(Vector3 destination, Vector3 position)
        {
            bool condition = true;
            Vector3 direction = (position - destination);
            direction.Normalize();
            foreach (Pieces b in ListeDesPièces)
            {
                Vector3 laDirection = (b.Position - destination);
                laDirection.Normalize();


                if ((Math.Abs(b.Position.Z - destination.Z) <= Math.Abs(position.Z - destination.Z)) && (Math.Abs(b.Position.X - destination.X) <= Math.Abs(position.X - destination.X)))
                {
                    if ((direction.X - 0.0001f < laDirection.X) && (laDirection.X < direction.X + 0.0001f) && (laDirection.Z - 0.0001f < direction.Z) && (laDirection.Z + 0.0001f > direction.Z))
                    {

                        if (b.Position != position)
                        {
                            condition = false;
                        }
                    }

                }

            }
            return condition;




        }
        private bool EstAuBorne(string Couleur, Cases Case)
        {
            bool estAuBorne = true;
            Vector3 destinationW = CaseB.Centre + new Vector3(2, 0, 0);
            Vector3 destinationB = CaseB.Centre + new Vector3(-2, 0, 0);
            if (Couleur == "White")
            {
                foreach (Cases f in ListeDesCases)
                {
                    if ((f.Centre == destinationW))
                    {
                        estAuBorne = false;
                    }
                }
            }
            else
            {
                foreach (Cases f in ListeDesCases)
                {
                    if ((f.Centre == destinationB))
                    {
                        estAuBorne = false;
                    }
                }

            }
            return estAuBorne;
        }
        private bool EstPropriétaire(Vector3 Position, Vector3 Position2)
        {
            return (Position == Position2);
        }
        private bool EstValidePionSeul(Cases A, Cases B)
        {
            return (CaseB.Centre.Z - CaseA.Centre.Z != 0);
        }
        private void ResetPièces(Pieces A, Pieces B)
        {
            PièceA.Deplacer(CaseA.Centre);
            PièceB.Deplacer(CaseB.Centre);
            Compteur--;
            ResetCouleur();

        }
        private void ResetPièces(Pieces A)
        {
            PièceA.Deplacer(CaseA.Centre);
            Compteur--;
            ResetCouleur();

        }
        private void EstValidePionMange(Cases A, Cases B)
        {
            Vector3 déplacement = (B.Centre - A.Centre);
            if (!PièceA.EstValidePion(déplacement))
            {
                ResetPièces(PièceA, PièceB);
            }

        }
        private void NouvelleReine(Pieces A)
        {
            Pieces nouvellePiece = PièceA.PromoteQueen();
            ListeDesPièces.Add(nouvellePiece);

            ListeDesPièces.Remove(PièceA);
        }

        private void ResetCouleur()
        {
            foreach (Cases t in ListeDesCases)
            {
                t.ResetCouleur();
            }
        }
        private void VérifierChangementCouleur(Cases A)
        {
            foreach (Pieces g in ListeDesPièces)
            {
                if (g.Position == A.Centre && g.Couleur == Couleur)
                {
                    A.ChangerCouleur(Color.Blue);
                }
            }
        }

        private void GèrerRook()
        {
            bool r = true;
            for (float i = 0; i <= 1; i += 0.5f)
            {
                PièceA.Deplacer(CaseA.Centre + i * (CaseB.Centre - CaseA.Centre));
                if (EstEnEchec(ListeDesCases, ListeDesPièces, PièceA.Couleur))
                {
                    r = false;
                }
            }
            bool aBouger = false;
            foreach (Pieces c in ListeDesPièces.FindAll(x => x.Nom == "/rook" && x.Couleur == PièceA.Couleur && x.EstPremierMove == true))
            {
                if (PièceA.EstPremierMove && r == true)
                {
                    if (c.Position == PièceA.Position + new Vector3(0, 0, 2))
                    {
                        c.Deplacer(CaseA.Centre + new Vector3(0, 0, 2));
                        aBouger = true;
                        PièceA.EstPremierMove = false;
                        ResetCouleur();

                    }

                    if (c.Position == PièceA.Position + new Vector3(0, 0, -4))
                    {
                        c.Deplacer(CaseA.Centre + new Vector3(0, 0, -2));
                        aBouger = true;
                        PièceA.EstPremierMove = false;
                        ResetCouleur();
                    }


                }



            }
            if (aBouger == false)
            {

                ResetPièces(PièceA);
                PièceA.EstPremierMove = true;
            }
        }
        private void GérerDéplacement()
        {
            if (GestionInput.EstNouveauClicGauche())
            {
                foreach (Cases o in ListeDesCases)
                {


                    Vector3 HG = Game.GraphicsDevice.Viewport.Project(o.HG, CaméraJeu.Projection, this.CaméraJeu.Vue, Matrix.Identity);
                    HG.X -= Game.GraphicsDevice.Viewport.X;
                    HG.Y -= Game.GraphicsDevice.Viewport.Y;

                    Vector3 HD = Game.GraphicsDevice.Viewport.Project(o.HD, this.CaméraJeu.Projection, this.CaméraJeu.Vue, Matrix.Identity);
                    HD.X -= Game.GraphicsDevice.Viewport.X;
                    HD.Y -= Game.GraphicsDevice.Viewport.Y;

                    Vector3 BG = Game.GraphicsDevice.Viewport.Project(o.BG, this.CaméraJeu.Projection, this.CaméraJeu.Vue, Matrix.Identity);
                    BG.X -= Game.GraphicsDevice.Viewport.X;
                    BG.Y -= Game.GraphicsDevice.Viewport.Y;

                    Vector3 BD = Game.GraphicsDevice.Viewport.Project(o.BD, this.CaméraJeu.Projection, this.CaméraJeu.Vue, Matrix.Identity);
                    BD.X -= Game.GraphicsDevice.Viewport.X;
                    BD.Y -= Game.GraphicsDevice.Viewport.Y;

                    int height = 0;
                    int width = 0;
                    Rectangle zone = new Rectangle(0, 0, 0, 0);

                    // int width = (int)(HD.Y-HG.Y);
                    // int height = (int)(BG.X-HG.X);
                    if (Couleur == BLACK)
                    {
                        height = (int)(HD.Y - HG.Y);
                        width = (int)(BG.X - HG.X);
                        zone = new Rectangle((int)HG.X, (int)HG.Y, width, height);
                    }
                    else
                    {
                        height = (int)(HG.Y - HD.Y);
                        width = (int)(HG.X - BG.X);
                        zone = new Rectangle((int)BG.X, (int)HD.Y, width, height);
                    }




                    //zone = new Rectangle((int)HG.X, (int)HG.Y, width,height);
                    Point PosSouris = GestionInput.GetPositionSouris();


                    float k = Compteur;
                    if (zone.Contains(PosSouris))
                    {

                        if (CaseA != CaseB)
                        {
                            VérifierChangementCouleur(o);
                        }
                        if (CaseA == null)
                        {


                            CaseA = o;
                            VérifierChangementCouleur(o);

                        }


                        else
                        {


                            CaseB = o;



                            foreach (Pieces a in ListeDesPièces.ToList())
                            {


                                if (CaseA != null && CaseB != null)
                                {

                                    foreach (Pieces b in ListeDesPièces)
                                    {
                                        if (EstPropriétaire(b.Position, CaseB.Centre))
                                        {
                                            PièceB = b;
                                        }
                                    }
                                    if (EstPropriétaire(a.Position, CaseA.Centre))
                                    {
                                        if (a.Couleur == Couleur)
                                        {
                                            if (a.LogiqueDéplacement(new Vector2(((int)CaseB.Centre.Z - (int)CaseA.Centre.Z), ((int)CaseB.Centre.X - (int)CaseA.Centre.X))) && NeSautePas(CaseA.Centre, CaseB.Centre))
                                            {
                                                Compteur++;
                                                PièceA = a;


                                                if (PièceB == null)
                                                {
                                                    ResetCouleur();
                                                    PièceA.Deplacer(CaseB.Centre);

                                                    if (PièceA.Nom == "/pawn")
                                                    {
                                                        // if (EstValidePionSeul(CaseA, CaseB))
                                                        if (EstValidePionSeul(CaseA, CaseB))
                                                        {
                                                            PièceA.Deplacer(CaseA.Centre);
                                                            Compteur--;
                                                            ResetCouleur();



                                                        }
                                                        else
                                                        {
                                                            if (EstAuBorne(PièceA.Couleur, CaseB))
                                                            {
                                                                NouvelleReine(PièceA);
                                                            }
                                                        }
                                                    }
                                                    if (PièceA.Nom == "/king")
                                                    {
                                                        if (Math.Abs(CaseB.Centre.Z - CaseA.Centre.Z) > 2 && (!EstEnEchec(ListeDesCases, ListeDesPièces, PièceA.Couleur)))
                                                        {
                                                            GèrerRook();
                                                        }
                                                    }


                                                    if (!EstPropriétaire(PièceA.Position, CaseA.Centre))
                                                    {
                                                        if (EstEnEchec(ListeDesCases, ListeDesPièces, Couleur))
                                                        {
                                                            PièceA.Deplacer(CaseA.Centre);
                                                            Compteur--;
                                                            ResetCouleur();


                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    if (PièceA.Couleur != PièceB.Couleur)
                                                    {
                                                        PièceB.Sortir(NbSortiesBlanc, NbSortiesNoir);

                                                        //   ListeDesPièces.Remove(PièceB);
                                                        //    NbPiece--;
                                                        PièceA.Deplacer(CaseB.Centre);
                                                        ResetCouleur();

                                                        if (PièceA.Nom == "/pawn")
                                                        {
                                                            Vector3 déplacement = (CaseB.Centre - CaseA.Centre);
                                                            if (!PièceA.EstValidePion(déplacement))
                                                            {
                                                                ResetPièces(PièceA, PièceB);
                                                                //      ListeDesPièces.Add(PièceB);
                                                                //     NbPiece++;
                                                            }
                                                            else
                                                            {
                                                                if (EstAuBorne(PièceA.Couleur, CaseB))
                                                                {
                                                                    NouvelleReine(PièceA);

                                                                }


                                                            }

                                                        }

                                                        if (PièceA.Nom == "/king")
                                                        {
                                                            if (Math.Abs((int)CaseB.Centre.Z - (int)CaseA.Centre.Z) > 2)
                                                            {
                                                                ResetPièces(PièceA, PièceB);
                                                            }
                                                        }




                                                        if (EstEnEchec(ListeDesCases, ListeDesPièces, Couleur))
                                                        {
                                                            ResetPièces(PièceA, PièceB);
                                                        }
                                                        else
                                                        {
                                                            // PièceB.Sortir(NbSortiesBlanc, NbSortiesNoir);
                                                            if (PièceB.Couleur == "White")
                                                            {
                                                                NbSortiesBlanc += 1;
                                                            }
                                                            else
                                                            {
                                                                NbSortiesNoir += 1;

                                                            }


                                                        }
                                                    }
                                                    else
                                                    {

                                                        Compteur--;
                                                        ResetCouleur();
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                ResetCouleur();

                                            }




                                        }
                                        else
                                        {
                                            ResetCouleur();
                                            VérifierChangementCouleur(CaseB);
                                            PièceA = PièceB;
                                            PièceB = null;
                                            CaseA = CaseB;
                                            CaseB = null;


                                        }

                                    }
                                    if (a == ListeDesPièces[NbPiece - 1])
                                    {
                                        ResetCouleur();
                                        if (CaseB != null)
                                        {
                                            VérifierChangementCouleur(CaseB);


                                        }



                                        PièceA = PièceB;
                                        PièceB = null;
                                        CaseA = CaseB;
                                        CaseB = null;







                                    }
                                    foreach (Pieces v in ListeDesPièces.ToList())
                                    {
                                        bool cd = true;
                                        foreach (Cases y in ListeDesCases)
                                        {
                                            if (v.Position == y.Centre)
                                            {
                                                cd = false;
                                            }
                                        }
                                        if (cd)
                                        {
                                            ListeDesPièces.Remove(v);
                                            NbPiece--;
                                        }
                                    }
                                    foreach (Pieces g in ListeDesPièces.FindAll(x => x.Nom == "/rook" || x.Nom == "/king" || x.Nom == "/pawn"))
                                    {
                                        if (g.NbDéplacement == 1)
                                        {
                                            g.EstPremierMove = false;
                                        }
                                    }



                                    // VerificationMat();







                                }



                            }

                            float p = Compteur;
                            if (k != p)
                            {

                                //CaméraJeu.TournerCaméra();
                                CaméraJeu.Compteur = 0;


                            }

                        }

                    }

                }

            }

        }

    }
}



