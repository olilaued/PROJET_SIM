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
    
    public class Tour : Microsoft.Xna.Framework.GameComponent
    {
       
        const string WHITE = "White";
        const string BLACK = "Black";
        float TempsÉcouléDepuisMAJ { get; set; }
        protected List<Cases> ListeDesCases { get; set; }
        protected List<Pieces> ListeDesPièces { get; set; }
        protected bool Action { get; set; }
        public string Couleur { get; set; }
        public string AutreCouleur { get; set; }
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
        RessourcesManager<SoundEffect> GestionnaireDeSons { get; set; }
        SoundEffect Chess_Hit_Sound { get; set; }
        SoundEffect Check_Sound { get; set; }
        SoundEffect Checkmate_Sound { get; set; }
        

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


       
        public override void Initialize()
        {
         
            TempsÉcouléDepuisMAJ = 0;
            Compteur = 0;
            PartieTerminée = false;
            CaméraJeu = Game.Services.GetService(typeof(Caméra)) as CaméraSubjective;
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GestionnaireDeSons = Game.Services.GetService(typeof(RessourcesManager<SoundEffect>)) as RessourcesManager<SoundEffect>;
            Chess_Hit_Sound = GestionnaireDeSons.Find("chess_sound");
            Check_Sound = GestionnaireDeSons.Find("check_sound");
            Checkmate_Sound = GestionnaireDeSons.Find("checkmate_sound");

            base.Initialize();
            
        }

        
        public override void Update(GameTime gameTime)
        {
            //LE COMPTEUR AUGMENTE DE 1 APRÈS CHAQUE TOUR POUR CHANGER DE JOUEUR
            if (Compteur % 2 == 0)
            {

                Couleur = WHITE;
                AutreCouleur = BLACK;
            }
            else
            {
                Couleur = BLACK;
                AutreCouleur = WHITE;
            }
            //ON VÉRIFIE QUE LA CAMÉRA A FINI DE TOURNER AVANT DE CONSIDÉRER LE CLICK
            if (CaméraJeu.aFiniTourner())
            {
                GérerDéplacement();
            }
           

            base.Update(gameTime);
        }

        
        void JouerSons()
        {
            if (EstMat())
            {
                Checkmate_Sound.Play();
            }
            if (EstEnEchec(AutreCouleur))
            {
                Check_Sound.Play();

            }
        }
        public bool EstMat()
        {
            bool t = true;
            //ON TROUVE L'ENSEMBLE DES PIÈCES DU JOUEUR QUI PEUT ÊTRE MAT
            foreach (Pieces l in ListeDesPièces.FindAll(x => x.Couleur != Couleur))
            {

                Pieces laPiece = null;
                Vector3 posPiece = Vector3.Zero;
                Vector3 posIni = l.Position;
                foreach (Cases r in ListeDesCases)
                {

                    //ON TROUVE L'ENSEMBLE DES DÉPLACEMENTS QUE PEUT FAIRE CETTE PIÈCE
                    if (l.LogiqueDéplacement(new Vector2((r.Centre.Z - l.Position.Z), (r.Centre.X - l.Position.X))) && NeSautePas(r.Centre, l.Position))
                    {

                        l.Deplacer(r.Centre);

                        //ON SORT LA PIÈCE QUI S'EST FAIT MANGER S'IL Y A LIEU
                        foreach (Pieces u in ListeDesPièces.FindAll(x => x.Couleur != l.Couleur))
                        {
                            if (u.Position == r.Centre)
                            {
                                laPiece = u;
                                posPiece = u.Position;
                                laPiece.Sortir(1000, 1000);


                            }
                        }

                       


                        foreach (Pieces q in ListeDesPièces.FindAll(x => x.Couleur == l.Couleur))
                        {
                            //SI LE DÉPLACEMENT IMPLIQUE DE MANGER UNE PIÈCE DE LA MAUVAISE COULEUR, ON RESET LA POSITION
                            if (l != q && q.Position == l.Position)
                            {
                                l.Deplacer(posIni);
                                
                            }

                        }
                        if (l.Nom == "/pawn")
                        {
                            
                            if (laPiece == null)
                            {
                                //IL N'Y A PAS EU DE CAPTURE, ON VÉRIFIE DONC LE CAS DU PION QUI SE DÉPLACE SANS MANGER                            
                                if ((r.Centre.X - posIni.X != 0) && (r.Centre.Z - posIni.Z != 0))
                                {
                                    l.Deplacer(posIni);
                                    
                                }
                            }


                        }
                        //ON VÉRIFIE QUE LE ROI NE ROQUE PAS CAR IL EST INVALIDE DE ROQUER LORSQU'ON EST EN ÉCHEC
                        if (l.Nom == "/king" && Math.Abs(r.Centre.Z - posIni.Z) > Partie.LONGUEUR_CASE)
                        {
                            l.Deplacer(posIni);
                            

                        }
                        //ON VÉRIFIE SI LE JOUEUR NE SERAIT PLUS EN ÉCHEC APRÈS AVOIR JOUÉ CE COUP
                        if (!EstEnEchec(l.Couleur))
                        {

                            t = false;

                        }
                        //ON RESET LA POSITION DE LA PIÈCE QUI A BOUGÉE ET DE CELLE CAPTURÉE CAR ON NE FAIT QUE VÉRIFIER LES CAS POSSIBLES
                        l.Deplacer(posIni);
                        if (laPiece != null)
                        {
                            laPiece.Deplacer(posPiece);
                        }                     

                    }

                }

            }
            //t EST FALSE SI IL EXISTE UN DÉPLACEMENT VALIDE POUR LEQUEL LE JOUEUR N'EST PAS EN ÉCHEC
            return t;


           

        }

        private bool EstEnEchec(string CouleurDuRoi)
        {
            bool condition = false;
            //ON TROUVE LE ROI DE LA COULEUR QUI PEUT ÊTRE ÉCHEC
            Pieces leRoi = ListeDesPièces.Find(x => x.Nom == "/king" && x.Couleur == CouleurDuRoi);


            foreach (Pieces a in ListeDesPièces)
            {
                //ON CRÉE UN VECTEUR DE DÉPLACEMENT ENTRE CHAQUE PIÈCE a ET LE ROI
                 Vector3 déplacement = new Vector3((leRoi.Position.X - a.Position.X), 0f, (leRoi.Position.Z - a.Position.Z));
                
               // ON VÉRIFIE SI CETTE PIÈCE EST CAPABLE DE MANGER LE ROI
                if (a.LogiqueDéplacement(new Vector2((leRoi.Position.Z - a.Position.Z), (leRoi.Position.X - a.Position.X))) && (a.Couleur != leRoi.Couleur) && NeSautePas(leRoi.Position, a.Position))
                {

                    condition = true;
                    if (a.Nom == "/pawn")
                    {
                        // ON VÉRIFIE LE CAS DU PION QUI SE DÉPLACE POUR MANGER
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
            //ON NORMALISE LA DIRECTION ENTRE LA PIÈCE QUI VEUT SE DÉPLACER ET LA DESTINATION
            direction.Normalize();
            foreach (Pieces b in ListeDesPièces)
            {
                Vector3 laDirection = (b.Position - destination);
                //ON NORMALISE LA DIRECTION ENTRE CHAQUE PIÈCE b ET LA DESTINATION
                laDirection.Normalize();
                

                //ON VÉRIFIE SI UNE PIÈCE b EST PLUS RAPPROCHÉE DE LA DESTINATION QUE LA PIÈCE a
                if ((Math.Abs(b.Position.Z - destination.Z) <= Math.Abs(position.Z - destination.Z)) && (Math.Abs(b.Position.X - destination.X) <= Math.Abs(position.X - destination.X)))
                {
                    //ON VÉRIFIE ENSUITE QUE CETTE PIÈCE À LA MÊME TRAJECTOIRE (ELLE EST DONC ENTRE LA PIÈCE a ET LA DESTINATION)
                    //PETIT INTERVALLE ENTRE LES DEUX VALEURS CAR ELLES NE SONT PAS TOUJOURS ÉXACTES
                    if ((direction.X - 0.0001f < laDirection.X) && (laDirection.X < direction.X + 0.0001f) && (laDirection.Z - 0.0001f < direction.Z) && (laDirection.Z + 0.0001f > direction.Z))
                    {
                        //ON VÉRIFIE QU'IL NE S'AGIT PAS DE NOTRE PIÈCE a
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
            Vector3 destinationW = CaseB.Centre + new Vector3(Partie.LONGUEUR_CASE, 0, 0);
            Vector3 destinationB = CaseB.Centre + new Vector3(-Partie.LONGUEUR_CASE, 0, 0);
            //ON DÉPLACE LE PION D'UNE CASE VERS L'AVANT
            if (Couleur == "White")
            {
                foreach (Cases f in ListeDesCases)
                {
                    //ON VÉRIFIE SI LE PION BLANC EST ENCORE SUR UNE CASE ( IL N'ÉTAIT DONC PAS À LA BORNE AVANT DE BOUGER)
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
                    //ON VÉRIFIE SI LE PION NOIR EST ENCORE SUR UNE CASE ( IL N'ÉTAIT DONC PAS À LA BORNE AVANT DE BOUGER)
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
        private void SetÉtat ()
        {
            foreach (Pieces v in ListeDesPièces)
            {
                v.PosIni = v.Position;
            }
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
                //ON VÉRIFIE SI LE ROI PASSE PAR UNE CASE SUR LAQUELLE IL EST ÉCHEC LORSQU'IL ESSAIE DE ROQUER
                PièceA.Deplacer(CaseA.Centre + i * (CaseB.Centre - CaseA.Centre));
                if (EstEnEchec(PièceA.Couleur))
                {
                    r = false;
                }
            }
            bool aBouger = false;
            foreach (Pieces c in ListeDesPièces.FindAll(x => x.Nom == "/rook" && x.Couleur == PièceA.Couleur && x.EstPremierMove == true))
            {
                if (PièceA.EstPremierMove == true && r == true)
                    // ON VÉRIFIE QUE LA TOUR ET LE ROI SONT À LEUR PREMIER DÉPLACEMENT
                {
                    if (c.Position == PièceA.Position + new Vector3(0, 0, Partie.LONGUEUR_CASE))
                    {
                        //SI LE ROI SE SITUE À LA BONNE DISTANCE D'UNE DES TOURS, ON FAIT LE DÉPLACEMENT
                        c.Deplacer(CaseA.Centre + new Vector3(0, 0, Partie.LONGUEUR_CASE));
                        aBouger = true;
                        PièceA.EstPremierMove = false;
                        ResetCouleur();

                    }

                    if (c.Position == PièceA.Position + new Vector3(0, 0, -2*Partie.LONGUEUR_CASE))
                    {
                        //ON VÉRIFIE LE ROQUE DE VERS L'AUTRE SENS
                        c.Deplacer(CaseA.Centre + new Vector3(0, 0, -Partie.LONGUEUR_CASE));
                        aBouger = true;
                        PièceA.EstPremierMove = false;
                        ResetCouleur();
                    }


                }



            }
            if (aBouger == false)
            {
                //SI LE ROQUE EST INVALIDE, ON RESET LE ROI ET ON LUI REDONNE SON PREMIER DÉPLACEMENT
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
                    //ON PROJETE LES VECTOR3 POUR OBTERNIR DES COORDONNÉES ÉCRAN

                    Vector3 HG = Game.GraphicsDevice.Viewport.Project(o.HG, CaméraJeu.Projection, this.CaméraJeu.Vue, Matrix.Identity);
             
                    Vector3 HD = Game.GraphicsDevice.Viewport.Project(o.HD, this.CaméraJeu.Projection, this.CaméraJeu.Vue, Matrix.Identity);
              
                    Vector3 BG = Game.GraphicsDevice.Viewport.Project(o.BG, this.CaméraJeu.Projection, this.CaméraJeu.Vue, Matrix.Identity);
              
                    Vector3 BD = Game.GraphicsDevice.Viewport.Project(o.BD, this.CaméraJeu.Projection, this.CaméraJeu.Vue, Matrix.Identity);
               

                    //ON CRÉE LES RECTANGLES DE COLLISIONS DES CASES
                    int height = 0;
                    int width = 0;
                    Rectangle zone = new Rectangle(0, 0, 0, 0);                
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




                   
                    Point PosSouris = GestionInput.GetPositionSouris();


                    float k = Compteur;
                    if (zone.Contains(PosSouris))
                    {
                        //COLLISION ENTRE SOURIS ET CASE

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
                            //LES DEUX CASES NE SONT PAS NULLES
                            CaseB = o;



                            foreach (Pieces a in ListeDesPièces.ToList())
                            {


                                if (CaseA != null && CaseB != null)
                                {
                                    //ON ENREGISTRE LA POSITION INITIALE DES PIÈCES
                                    SetÉtat();

                                    foreach (Pieces b in ListeDesPièces)
                                    {
                                        if (EstPropriétaire(b.Position, CaseB.Centre))
                                        {
                                            //ON TROUVE LA PIÈCE SE TROUVANT SUR caseB
                                            PièceB = b;
                                        }
                                    }
                                    if (EstPropriétaire(a.Position, CaseA.Centre))
                                    {
                                        //ON TROUVE LA PIÈCE SUR LA caseA ET ON VÉRIFIE SI ELLE RESPECTE LOGIQUE ET NESAUTEPAS
                                        if (a.Couleur == Couleur)
                                        {
                                            if (a.LogiqueDéplacement(new Vector2((CaseB.Centre.Z - CaseA.Centre.Z), (CaseB.Centre.X - CaseA.Centre.X))) && NeSautePas(CaseA.Centre, CaseB.Centre))
                                            {
                                                
                                                Compteur++;
                                                PièceA = a;


                                                if (PièceB == null)
                                                {
                                                    //ON PASSE ICI SI IL N'Y A PAS DE pièceB (DÉPLACEMENT SANS MANGER)
                                                    ResetCouleur();
                                                    PièceA.Deplacer(CaseB.Centre);

                                                    if (PièceA.Nom == "/pawn")
                                                    {
                                                        //ON VÉRIFIE LE CAS DU DÉPLACEMENT DU PION SANS MANGER
                                                        if (EstValidePionSeul(CaseA, CaseB))
                                                        {
                                                            PièceA.Deplacer(CaseA.Centre);
                                                            Compteur--;
                                                           
                                                            ResetCouleur();



                                                        }
                                                        else
                                                        {
                                                            //ON VÉRIFIE SI LE PION EST AUX BORNES (PROMOTION)
                                                            if (EstAuBorne(PièceA.Couleur, CaseB))
                                                            {
                                                                NouvelleReine(PièceA);
                                                            }
                                                        }
                                                    }
                                                    if (PièceA.Nom == "/king")
                                                    {
                                                        //ON VÉRIFIE SI LE JOUEUR ESSAIE DE ROQUER
                                                        if (Math.Abs(CaseB.Centre.Z - CaseA.Centre.Z) > Partie.LONGUEUR_CASE && (!EstEnEchec( PièceA.Couleur)))
                                                        {
                                                            GèrerRook();
                                                        }
                                                    }


                                                    if (!EstPropriétaire(PièceA.Position, CaseA.Centre))
                                                    {
                                                        // SI pièceA A BOUGÉ, ON VÉRIFIE SI LE JOUEUR SE MET LUI-MÊME EN ÉCHEC
                                                        if (EstEnEchec(Couleur))
                                                        {
                                                            //ON REMET LA PIÈCE À SA POSITION INITIALE CAR LE MOUVEMENT EST INVALIDE (SE MET EN ÉCHEC)
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
                                                        //ON PASSE ICI SI LA pièceA VEUT MANGER LA pièceB
                                                        PièceB.Sortir(NbSortiesBlanc, NbSortiesNoir);

                                                        
                                                        PièceA.Deplacer(CaseB.Centre);
                                                        ResetCouleur();

                                                        if (PièceA.Nom == "/pawn")
                                                        {
                                                            //ON VÉRIFIE LE CAS UNIQUE DU PION QUI MANGE UNE PIÈCE
                                                            Vector3 déplacement = (CaseB.Centre - CaseA.Centre);
                                                            if (!PièceA.EstValidePion(déplacement))
                                                            {
                                                                //ON RESET LES PIÈCES SI LE DÉPLACEMENT DU PION EST INVALIDE
                                                                ResetPièces(PièceA, PièceB);
                                                               
                                                            }
                                                            else
                                                            {
                                                                if (EstAuBorne(PièceA.Couleur, CaseB))
                                                                {
                                                                    //ON PROMOTE LE PION SI IL EST AUX BORNES
                                                                    NouvelleReine(PièceA);

                                                                }


                                                            }

                                                        }

                                                        if (PièceA.Nom == "/king")
                                                        {
                                                            //IMPOSSIBLE DE ROQUER LORSQU'ON MANGE UNE PIÈCE, ON VÉRIFIE QUE LE JOUEUR N'ESSAIE PAS DE ROQUER
                                                            if (Math.Abs(CaseB.Centre.Z - CaseA.Centre.Z) > Partie.LONGUEUR_CASE)
                                                            {
                                                                ResetPièces(PièceA, PièceB);
                                                               
                                                            }
                                                        }




                                                        if (EstEnEchec(Couleur))
                                                        {
                                                           //ON RESET LES PIÈCES SI LE JOUEUR S'EST MIS EN ÉCHEC
                                                            ResetPièces(PièceA, PièceB);
                                                        }
                                                        else
                                                        {      
                                                            //ON MODIFIE LA POSITION DE SORTIE PUISQU'UNE PIÈCE EST SORTIE SI ON PASSE ICI
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
                                                        //ON RESET LA PIÈCE CAR LE JOEUEUR A TENTÉ DE MANGER UNE PIÈCE DE LA MAUVAISE COULEUR

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
                                            //ON UTILISE LA pièceB comme pièceA CAR LE JOUEUR A CLICKÉ SUR DEUX PIÈCES DE SA COULEUR
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
                                        //LE JOUEUR A PRIS UNE CASE VIDE COMME caseA, ON MET DONC LA caseB COMME ÉTANT caseA
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
                               


                                    foreach (Pieces g in ListeDesPièces.FindAll(x => x.Nom == "/rook" || x.Nom == "/king" || x.Nom == "/pawn"))
                                    {
                                        //POUR LES PIÈCES QUI DOIVENT SAVOIR SI ELLES SONT À LEUR PREMIER DÉPLACEMENT, ON VÉRIFIE SI ELLES ONT BOUGÉ
                                        if (g.Position != g.PosIni)
                                        {
                                            g.EstPremierMove = false;
                                        }
                                    }


                                   


                                }



                            }
                                                      
                            float p = Compteur;
                            if (k != p)
                            {
                                //ON VÉRIFIE À L'AIDE DU COMPTEUR SI LE COUP A ÉTÉ JOUÉ, ON FAIT TOURNER LA CAMÉRA ET ON JOUE LE SON DE DÉPLACEMENT
                               
                                CaméraJeu.Compteur = 0;
                                Chess_Hit_Sound.Play();


                            }
                            //ON VÉRIFIE SI IL FAUT JOUER LES SONS ÉCHEC/ÉCHEC ET MAT
                            JouerSons();

                           


                        }

                    }

                }

            }

        }

    }
}



