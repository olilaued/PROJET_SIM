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
    public class Partie : Microsoft.Xna.Framework.GameComponent
    {
        const float LARGEUR_ECHIQUIER = 15f;
        const float LONGUEUR_ÉCHIQUIER = 16f;
       
        List<Pieces> ListeDesPièces { get; set; }
        protected List<string> ListeDesMoves { get; set; }
        protected float TempsLimite { get; set; }
        protected Model Map { get; set; }
        protected Echiquier UnÉchiquier { get; set; }
        protected CaméraSubjective CaméraJeu { get; set; }
        protected float NbSortiesBlanc { get; set; }
        protected float NbSortiesNoir { get; set; }
        protected float TempsÉcoulé { get; set; }
        protected Tour TourActuel { get; set; }

        InputManager GestionInput { get; set; }
        
        






        public Partie(Game game, float tempsLimite, Model map, Color[] couleursÉchiquier, Vector3 origineÉchiquier)
            : base(game)
        {
            UnÉchiquier = new Echiquier(Game, origineÉchiquier, new Vector2(LONGUEUR_ÉCHIQUIER, LARGEUR_ECHIQUIER), couleursÉchiquier[0],
                couleursÉchiquier[1], couleursÉchiquier[2]);
            TempsLimite = tempsLimite;
            Map = map;
        }
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            NbSortiesBlanc = 0;
            NbSortiesNoir = 0;
            InitialiserPièces(UnÉchiquier);
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            CaméraJeu = Game.Services.GetService(typeof(CaméraSubjective)) as CaméraSubjective;
            TourActuel = new Tour(Game,"White", UnÉchiquier.ListeCases, ListeDesPièces, NbSortiesBlanc, NbSortiesNoir);
            Game.Components.Add(TourActuel);
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {

            if ( TempsÉcoulé < TempsLimite)
            {
                

            }




            base.Update(gameTime);
        }
        void InitialiserPièces(Echiquier unEchiquier)
        {
            ListeDesPièces = new List<Pieces>();
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
    }
        
    
}
