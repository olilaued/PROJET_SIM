﻿using System;
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
    /// This is the main type for your game
    /// </summary>
    public class Jeu : Microsoft.Xna.Framework.Game
    {
         const float LARGEUR_ECHIQUIER = 16f;
         
        const float INTERVALLE_CALCUL_FPS = 1f;
        const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
        GraphicsDeviceManager PériphériqueGraphique { get; set; }
        SpriteBatch GestionSprites { get; set; }

        RessourcesManager<SpriteFont> GestionnaireDeFonts { get; set; }
        RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
        RessourcesManager<Model> GestionnaireDeModèles { get; set; }
        RessourcesManager<Effect> GestionnaireDeShaders { get; set; }
        InputManager GestionInput { get; set; }
        Caméra CaméraJeu { get; set; }

        public List<Pieces> ListePièces { get; set; }

       
        

        public Jeu()
        {
            PériphériqueGraphique = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            ListePièces = new List<Pieces>();
            
            // TODO: Add your initialization logic here
            Vector3 positionCaméra = Vector3.Zero;
            Vector3 positionObjet = new Vector3(0, 0, 0);
            Vector3 rotationObjet = new Vector3(0, 0, 0);
            
            

            GestionnaireDeFonts = new RessourcesManager<SpriteFont>(this, "Fonts");
            GestionnaireDeTextures = new RessourcesManager<Texture2D>(this, "Textures");
            GestionnaireDeModèles = new RessourcesManager<Model>(this, "Models");
            GestionnaireDeShaders = new RessourcesManager<Effect>(this, "Effects");
            GestionInput = new InputManager(this);
            CaméraJeu = new CaméraSubjective(this, new Vector3(0,0,8), positionObjet, Vector3.Up, INTERVALLE_MAJ_STANDARD);
            
          
            Echiquier unEchiquier = new Echiquier(this, new Vector3(0, 0, 0), new Vector2(LARGEUR_ECHIQUIER, 0.3f), Color.BurlyWood,Color.MediumSeaGreen, Color.Blue);
            Components.Add(unEchiquier);
           
            Components.Add(CaméraJeu);

            InitialiserPièces(unEchiquier);

            //ObjetDeBase table = new ObjetDeBase(this, "Pub/table", 0.05f, rotationObjet, new Vector3(5,5,5));
            //Components.Add(table);
            //ObjetDeBase divan = new ObjetDeBase(this, "Pub/Divan", 0.05f, rotationObjet, new Vector3(-5, 0, 0));
            //Components.Add(divan);
            ObjetDeBase maison = new ObjetDeBase(this, "Pub/Club_map_2", 0.009f, rotationObjet, positionObjet);
            Components.Add(maison);
           // Pions pionB = new Pions(this,positionObjet, "Black");
            //Components.Add(pionB);

            
            
            
            Components.Add(new Afficheur3D(this));
            Components.Add(GestionInput);

           

            

            Services.AddService(typeof(Random), new Random());
            Services.AddService(typeof(RessourcesManager<SpriteFont>), GestionnaireDeFonts);
            Services.AddService(typeof(RessourcesManager<Texture2D>), GestionnaireDeTextures);
            Services.AddService(typeof(RessourcesManager<Model>), GestionnaireDeModèles);
            Services.AddService(typeof(RessourcesManager<Effect>), GestionnaireDeShaders);
            Services.AddService(typeof(InputManager), GestionInput);
            Services.AddService(typeof(Caméra), CaméraJeu);
            GestionSprites = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), GestionSprites);
            base.Initialize();
            
        }
        void InitialiserPièces(Echiquier unEchiquier)
        {
            for (int i = 0; i < 8; i++)
            {
                Pions pionB = new Pions(this, unEchiquier.ListeCases[1 + 8 * i].Centre, "Black");
                ListePièces.Add(pionB);
                Pions pionW = new Pions(this, unEchiquier.ListeCases[(1 + 8 * i)+5].Centre, "White");
                ListePièces.Add(pionW);

            }
            for (int i = 0; i < 2; i++)
            {
                //CRÉATION TOURS
                Tours tourB = new Tours(this, unEchiquier.ListeCases[0 + 56 * i].Centre, "Black");
                ListePièces.Add(tourB);
                Tours tourW = new Tours(this,unEchiquier.ListeCases[(0+56*i)+7].Centre,"White");
                ListePièces.Add(tourW);

                //CRÉATION CAVALIERS
                Cavaliers cavalierB = new Cavaliers(this, unEchiquier.ListeCases[8 + 40 * i].Centre, "Black");
                ListePièces.Add(cavalierB);
                Cavaliers cavalierW = new Cavaliers(this, unEchiquier.ListeCases[(8 + 40 * i)+7].Centre, "White");
                ListePièces.Add(cavalierW);

                //CRÉATION FOUS
                Fous fouB = new Fous(this, unEchiquier.ListeCases[16 + 24 * i].Centre, "Black");
                ListePièces.Add(fouB);
                Fous fouW = new Fous(this, unEchiquier.ListeCases[(16 + 24 * i)+7].Centre, "White");
                ListePièces.Add(fouW);
                
                //CRÉATION REINES
                Reine reineB = new Reine(this, unEchiquier.ListeCases[24].Centre, "Black");
                ListePièces.Add(reineB);
                Reine reineW = new Reine(this, unEchiquier.ListeCases[24+7].Centre, "White");
                ListePièces.Add(reineW);
                
                //CRÉATION ROI
                Roi roiB = new Roi(this, unEchiquier.ListeCases[32].Centre, "Black");
                ListePièces.Add(roiB);
                Roi roiW = new Roi(this, unEchiquier.ListeCases[32+7].Centre, "White");
                ListePièces.Add(roiW);
            }
            for (int i = 0; i< 2 ; i++)
            {
                


            }
            for (int i =0; i<2; i++)
            {
                

            }
            for (int i = 0; i<2; i++)
            {
               
                
            }
            for (int i = 0; i<2; i++)
            {
             

            }
            
            

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            
           

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
          
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            // TODO: Add your drawing code here
            
            base.Draw(gameTime);
        }
    }
}
