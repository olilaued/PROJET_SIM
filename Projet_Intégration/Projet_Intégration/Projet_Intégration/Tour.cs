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
        private int NbPiece { get; set; }

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
                Vector3 déplacement = new Vector3((leRoi.Position.X - a.Position.X), 0f, (leRoi.Position.Z - a.Position.Z));
                if (a.LogiqueDéplacement(new Vector2((leRoi.Position.X - a.Position.X), (leRoi.Position.Z - a.Position.Z))) && (a.Couleur != leRoi.Couleur) && NeSautePas(leRoi.Position,a.Position))
                {

                    condition = true;
                    if (a.Nom == "/pawn")
                    {
                        condition = a.EstValidePion(déplacement);
                    }
                }



            }
            return condition;
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
                    if ((direction.X-0.0001f < laDirection.X) && (laDirection.X< direction.X+0.0001f) && (laDirection.Z-0.0001f  < direction.Z) &&( laDirection.Z+0.0001f > direction.Z))
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
            Vector3 destinationW = CaseB.Centre + new Vector3(0, 0, -2);
            Vector3 destinationB = CaseB.Centre + new Vector3(0, 0, 2);
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

                            foreach (Pieces a in ListeDesPièces.ToList())
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
                                                        else
                                                        {
                                                            if (EstAuBorne(PièceA.Couleur, CaseB))
                                                            {
                                                                Pieces nouvellePiece = PièceA.PromoteQueen();
                                                                ListeDesPièces.Add(nouvellePiece);
                                                                ListeDesPièces.Remove(PièceA);

                                                            }
                                                        }
                                                    }
                                                    if (PièceA.Nom == "/king")
                                                    {
                                                        if (Math.Abs(CaseB.Centre.X - CaseA.Centre.X )> 2 && (!EstEnEchec(ListeDesCases,ListeDesPièces,PièceA.Couleur)))
                                                           
                                                        {
                                                            bool r = true; 
                                                            for (float i =0; i <=1; i+=0.5f)
                                                            {
                                                                PièceA.Deplacer(CaseA.Centre + i * (CaseB.Centre - CaseA.Centre));
                                                                if (EstEnEchec(ListeDesCases,ListeDesPièces,PièceA.Couleur))
                                                                {
                                                                    r = false;
                                                                }
                                                            }
                                                            bool aBouger = false;
                                                            foreach (Pieces c in ListeDesPièces.FindAll(x=> x.Nom == "/rook" && x.Couleur == PièceA.Couleur && x.EstPremierMove == true))
                                                            {
                                                                if (PièceA.EstPremierMove && r == true)
                                                                {
                                                                    if (c.Position == PièceA.Position + new Vector3(2,0,0))
                                                                    {
                                                                        c.Deplacer(CaseA.Centre + new Vector3(2, 0, 0));
                                                                        aBouger = true;
                                                                        PièceA.EstPremierMove = false;
                                                                        
                                                                    }                                                                

                                                                    if (c.Position == PièceA.Position + new Vector3(-4, 0, 0))
                                                                    {
                                                                        c.Deplacer(CaseA.Centre + new Vector3(-2, 0, 0));
                                                                        aBouger = true;
                                                                        PièceA.EstPremierMove = false;
                                                                    }
                                                                    
                                                                    
                                                                }
                                                                
                                                                    

                                                            }
                                                            if( aBouger == false)
                                                            {
                                                                
                                                                PièceA.Deplacer(CaseA.Centre);
                                                                Compteur--;
                                                                PièceA.EstPremierMove = true;
                                                            }
                                                            
                                                        }
                                                    }

                                                    if (PièceA.Position != CaseA.Centre)
                                                    {
                                                        if (EstEnEchec(ListeDesCases, ListeDesPièces, Couleur))
                                                        {

                                                            PièceA.Deplacer(CaseA.Centre);
                                                            Compteur--;
                                                        }
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
                                                            else
                                                            {
                                                                if (EstAuBorne(PièceA.Couleur,CaseB))
                                                                {
                                                                    Pieces nouvellePiece = PièceA.PromoteQueen();
                                                                    ListeDesPièces.Add(nouvellePiece);
                                                                    ListeDesPièces.Remove(PièceA);
                                                                    
                                                                }
                                                               
                                                                
                                                            }
                                                            
                                                        }
                                                        
                                                        if (PièceA.Nom == "/king")
                                                        {
                                                             if (Math.Abs(CaseB.Centre.X - CaseA.Centre.X )> 2)
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
                                                    else
                                                    {

                                                        Compteur--;
                                                    }
                                                }

                                            }
                                        }
                                        else
                                        {
                                            PièceA = PièceB;
                                            PièceB = null;
                                            CaseA = CaseB;
                                            CaseB = null;
                                        }

                                        //PièceA = null;
                                        //PièceB = null;
                                        //CaseA = null;
                                        //CaseB = null;
                                    }
                                    if (a == ListeDesPièces[NbPiece-1])
                                    {
                                        PièceA = PièceB;
                                        PièceB = null;
                                        CaseA = CaseB;
                                        CaseB = null;
                                    }
                                    foreach (Pieces g in ListeDesPièces.FindAll(x => x.Nom == "/rook" || x.Nom == "/king"))
                                    {
                                        if (g.NbDéplacement == 1)
                                        {
                                            g.EstPremierMove = false;
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
}



