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
        protected float TempsÉcoulé { get; set; }
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

        public Tour(Game game, string couleur, List<Cases> listeCases, List<Pieces> listePièce, float nbSortiesBlanc, float nbSortiesNoir)
            : base(game)
        {
            Couleur = couleur;
            ListeDesCases = listeCases;
            ListeDesPièces = listePièce;
            NbSortiesBlanc = nbSortiesBlanc;
            NbSortiesNoir = nbSortiesNoir;
        }


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            TempsÉcoulé = 0;
            Compteur = 0;
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

            base.Update(gameTime);
        }
        private bool EstEnEchec(List<Cases> listeCases, List<Pieces> listePieces, string CouleurDuRoi)
        {
            bool condition = false;
            Pieces leRoi = ListeDesPièces.Find(x => x.Nom == "/king" && x.Couleur == CouleurDuRoi);
            

            foreach (Pieces a in ListeDesPièces)
            {
                if (a.LogiqueDéplacement(new Vector2((leRoi.Position.X - a.Position.X), (leRoi.Position.Z - a.Position.Z))) && (a.Couleur != leRoi.Couleur) && NeSautePas(a.Position,leRoi.Position))
                {
                    condition = true;
                }



            }
            return condition;
        }
        private bool NeSautePas( Vector3 caseA, Vector3 caseB)
        {
            bool condition = true;
            Vector3 direction = (caseB- caseA);
            direction.Normalize();
            foreach (Pieces a in ListeDesPièces)
            {
               Vector3 laDirection = (a.Position - caseA);
               laDirection.Normalize();
                if (laDirection == direction && Math.Abs(a.Position.Z - caseA.Z)<= Math.Abs(caseB.Z-caseA.Z) && ( Math.Abs(a.Position.X - caseA.X)<= Math.Abs(caseB.X-caseA.X)))
                {
                    if (a.Position != caseB && a.Nom != "/knight")
                    {
                        condition = false;
                    }
                }

            }
            return condition;

          
           
           
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

                    int width = (int)(HG.X - HD.X);
                    int height = (int)(BG.Y - HG.Y);




                    Rectangle zone = new Rectangle((int)BD.X, (int)HG.Y, width, height);
                    Point PosSouris = GestionInput.GetPositionSouris();



                    if (zone.Contains(PosSouris))
                    {
                        if (CaseA == null)
                        {
                            CaseA = o;
                        }
                        else
                        {
                            CaseB = o;

                            foreach (Pieces a in ListeDesPièces)
                            {
                                if (CaseA != null && CaseB != null)
                                {
                                    foreach (Pieces b in ListeDesPièces)
                                    {
                                        if (b.Position == CaseB.Centre)
                                        {
                                            PièceB = b;
                                        }
                                    }
                                    if (a.Position == CaseA.Centre)
                                    {



                                        if (a.Couleur == Couleur)
                                        {
                                            if (a.LogiqueDéplacement(new Vector2((CaseB.Centre.X - CaseA.Centre.X), (CaseB.Centre.Z - CaseA.Centre.Z))) && NeSautePas(CaseA.Centre, CaseB.Centre))
                                            {                                            
                                                Compteur++;
                                                PièceA = a;

                                                if (PièceB == null)
                                                {
                                                                                                       
                                                                                                  
                                                     PièceA.Deplacer(CaseB.Centre);
                                                    if (PièceA.Nom == "/pawn")
                                                    {
                                                        if (CaseB.Centre.X - CaseA.Centre.X != 0)
                                                        {
                                                            PièceA.Deplacer(CaseA.Centre);
                                                            Compteur--;
                                                        }
                                                    }

                                                         if (EstEnEchec(ListeDesCases, ListeDesPièces, Couleur))
                                                         {
                                                             PièceA.Deplacer(CaseA.Centre);
                                                             Compteur--;
                                                         }
                                                    
                                                }
                                                else
                                                {
                                                    if (PièceA.Couleur != PièceB.Couleur)
                                                    {
                                                        PièceB.Sortir(NbSortiesBlanc, NbSortiesNoir);

                                                        PièceA.Deplacer(CaseB.Centre);
                                                        if (PièceA.Nom == "/pawn")
                                                        {
                                                            Vector3 déplacement = (CaseB.Centre - CaseA.Centre);
                                                            if (!PièceA.EstValidePion(déplacement))
                                                            {
                                                                PièceA.Deplacer(CaseA.Centre);
                                                                PièceB.Deplacer(CaseB.Centre);
                                                                Compteur--;
                                                            }
                                                        }
                                                        if (EstEnEchec(ListeDesCases, ListeDesPièces, Couleur))
                                                        {
                                                            PièceA.Deplacer(CaseA.Centre);
                                                            PièceB.Deplacer(CaseB.Centre);
                                                            Compteur--;
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
                                                }

                                            }
                                        }

                                            PièceA = null;
                                            PièceB = null;
                                            CaseA = null;
                                            CaseB = null;
                                        }
                                        if (a == ListeDesPièces[31])
                                        {
                                            PièceA = PièceB;
                                            PièceB = null;
                                            CaseA = CaseB;
                                            CaseB = null;
                                        }
                                    }

                                }
                            }



                        }

                    }
                }
            }
        }
    }



