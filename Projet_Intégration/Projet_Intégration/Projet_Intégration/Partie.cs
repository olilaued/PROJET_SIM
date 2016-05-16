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
    
    public class Partie : Microsoft.Xna.Framework.GameComponent
    {
        const float LARGEUR_ECHIQUIER = 0.3f;
        public static float LONGUEUR_ÉCHIQUIER = 15f;
        public static float LONGUEUR_CASE = LONGUEUR_ÉCHIQUIER / 8f;
        

        public static float TempsLimite { get; set; }
        public Tour TourActuel { get; set; }
        public bool PartieTerminée {get; set;}
        List<Pieces> ListeDesPièces { get; set; }
        string Map { get; set; }
        Echiquier UnÉchiquier { get; set; }
        CaméraSubjective CaméraJeu { get; set; }
        float NbSortiesBlanc { get; set; }
        float NbSortiesNoir { get; set; }
        float TempsÉcouléDepuisDébut { get; set; }
        ObjetDeBase Environnement {get; set;}
        static Vector3[] PositionSorties { get; set; }
        TexteAffichable GagnantB { get; set; }
        TexteAffichable GagnantN { get; set; }
        
       

        InputManager GestionInput { get; set; }
        
        






        public Partie(Game game, float tempsLimite, string map, Color[] couleursÉchiquier, Vector3 origineÉchiquier)
            : base(game)
        {
           game.Components.Add(UnÉchiquier = new Echiquier(Game, origineÉchiquier, new Vector2(LONGUEUR_ÉCHIQUIER, LARGEUR_ECHIQUIER), couleursÉchiquier[0],
                couleursÉchiquier[1], couleursÉchiquier[2]));
            TempsLimite = tempsLimite;
            Map = map;
        }
        
        public override void Initialize()
        {
            PositionSorties = new Vector3[2];
            ListeDesPièces = new List<Pieces>();
            TempsÉcouléDepuisDébut = 0;
            NbSortiesBlanc = 0;
            NbSortiesNoir = 0;
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            CaméraJeu = Game.Services.GetService(typeof(CaméraSubjective)) as CaméraSubjective;
            PositionSorties[0] = new Vector3(UnÉchiquier.Origine.X, UnÉchiquier.Origine.Y, UnÉchiquier.Origine.Z + Partie.LONGUEUR_ÉCHIQUIER - LONGUEUR_CASE / 3f);
            PositionSorties[1] = new Vector3(UnÉchiquier.Origine.X - LONGUEUR_ÉCHIQUIER + 2 * LONGUEUR_CASE, UnÉchiquier.Origine.Y, UnÉchiquier.Origine.Z - 2 * LONGUEUR_CASE + LONGUEUR_CASE / 3f);
            Game.Components.Add(Environnement = new ObjetDeBase(Game, Map, 0.01f, new Vector3(0, 0, 0), Vector3.Zero));
            Game.Components.Add(TourActuel = new Tour(Game,"White", UnÉchiquier.ListeCases, ListeDesPièces, NbSortiesBlanc, NbSortiesNoir));
            Environnement.Visible = false;
            TourActuel.Enabled = false;
            InitialiserPièces(UnÉchiquier);
            
            
           
            base.Initialize();
        }

        
        public override void Update(GameTime gameTime)
        {
            
            float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisDébut += TempsÉcoulé;

            if (Jeu.CurrentGameState == Jeu.GameState.EnJeu)
            {
                if (Environnement.Visible == false)
                {
                    TourActuel.Enabled = true;
                    Environnement.Visible = true;
                    ModifierEstVisiblePièces();
                    UnÉchiquier.ModifierVisibilitéCases();
                }


                if (TempsÉcouléDepuisDébut > TempsLimite || TourActuel.PartieTerminée || TourActuel.EstMat())
                {
                    
                    PartieTerminée = true;
                }
            }
            if (Jeu.CurrentGameState == Jeu.GameState.EnPause && Environnement.Visible == true)
            {
                UnÉchiquier.ModifierVisibilitéCases();
                ModifierEstVisiblePièces();
                Environnement.Visible = false;
                Environnement.Enabled = false;
            }
            base.Update(gameTime);

        }
        void InitialiserPièces(Echiquier unEchiquier)
        {
            
            for (int i = 0; i < 8; i++)
            {
                Pions pionB = new Pions(Game, unEchiquier.ListeCases[1 + 8 * i].Centre, "Black");
                ListeDesPièces.Add(pionB);
                Pions pionW = new Pions(Game, unEchiquier.ListeCases[(1 + 8 * i) + 5].Centre, "White");
                ListeDesPièces.Add(pionW);


            }
            for (int i = 0; i < 2; i++)
            {
                //CRÉATION TOURS
                Tours tourB = new Tours(Game, unEchiquier.ListeCases[0 + 56 * i].Centre, "Black");
                ListeDesPièces.Add(tourB);
                Tours tourW = new Tours(Game, unEchiquier.ListeCases[(0 + 56 * i) + 7].Centre, "White");
                ListeDesPièces.Add(tourW);
                

                //CRÉATION CAVALIERS
                Cavaliers cavalierB = new Cavaliers(Game, unEchiquier.ListeCases[8 + 40 * i].Centre, "Black");
                ListeDesPièces.Add(cavalierB);
                Cavaliers cavalierW = new Cavaliers(Game, unEchiquier.ListeCases[(8 + 40 * i) + 7].Centre, "White");
                ListeDesPièces.Add(cavalierW);

                //CRÉATION FOUS
                Fous fouB = new Fous(Game, unEchiquier.ListeCases[16 + 24 * i].Centre, "Black");
                ListeDesPièces.Add(fouB);
                Fous fouW = new Fous(Game, unEchiquier.ListeCases[(16 + 24 * i) + 7].Centre, "White");
                ListeDesPièces.Add(fouW);
               

            }
            //CRÉATION REINES
            Reine reineB = new Reine(Game, unEchiquier.ListeCases[24].Centre, "Black");
            ListeDesPièces.Add(reineB);
            Reine reineW = new Reine(Game, unEchiquier.ListeCases[24 + 7].Centre, "White");
            ListeDesPièces.Add(reineW);

            //CRÉATION ROI
            Roi roiB = new Roi(Game, unEchiquier.ListeCases[32].Centre, "Black");
            ListeDesPièces.Add(roiB);
            Roi roiW = new Roi(Game, unEchiquier.ListeCases[32 + 7].Centre, "White");
            ListeDesPièces.Add(roiW);

        }
        
        public static Vector3 GetPositionSorties(int index)
        {
            
            if (index >= 0 && index < PositionSorties.Length)
            {
                return PositionSorties[index];
            }
            else
            {
                return Vector3.Zero;
            }
            
        }

        public void ModifierEstVisiblePièces()
        {
           foreach (Pieces p in ListeDesPièces)
            {
                p.Visible = !p.Visible;
            }

        }
        public void Retirer()
        {
            Game.Components.Remove(Environnement);
            foreach (Pieces p in ListeDesPièces)
            {
                Game.Components.Remove(p);
            }
            UnÉchiquier.Retirer();
            Game.Components.Remove(TourActuel);
            Game.Components.Remove(this);
        }

        
        
        
    
    }
        
    
}
