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
    /// This is the main type for your game
    /// </summary>
    public class Jeu : Microsoft.Xna.Framework.Game
    {

        public static bool EstVisible { get; set; }
        const float INTERVALLE_CALCUL_FPS = 1f;
        const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
        float Bordures { get; set; }
        
        GraphicsDeviceManager PériphériqueGraphique { get; set; }
        SpriteBatch GestionSprites { get; set; }

        RessourcesManager<SpriteFont> GestionnaireDeFonts { get; set; }
        RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
        RessourcesManager<Model> GestionnaireDeModèles { get; set; }
        RessourcesManager<Effect> GestionnaireDeShaders { get; set; }
        InputManager GestionInput { get; set; }
        Caméra CaméraJeu { get; set; }
        Color[] CouleursÉchiquier { get; set; }
        Partie PartiEnCours { get; set; }
        Afficheur3D unAfficheur3D { get; set; }
        List<Bouton> ListeDesBoutons { get; set; }
        string NomMap { get; set; }
        float TempsDePartie { get; set; }
        Vector3 OrigineÉchiquier { get; set; }

        // Menu principal
        Bouton Bouton1 { get; set; }
        Bouton Bouton2 { get; set; }
        Bouton Bouton3 { get; set; }

        string text1 = "Jouer En Solo";
        string text2 = "Jouer 1v1";
        string text3 = "Options";

        // Options
        Bouton Bouton4 { get; set; }
        Bouton Bouton5 { get; set; }
        Bouton Bouton6 { get; set; }

        string text4 = "Pub";
        string text5 = "Parc";
        string text6 = "Espace";
        
       
       


       

        
        
        



       
        

        public Jeu()
        {
            PériphériqueGraphique = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = false;
            PériphériqueGraphique.SynchronizeWithVerticalRetrace = false;
            PériphériqueGraphique.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            ListeDesBoutons = new List<Bouton>();
            TempsDePartie = 15 * 60;
            NomMap = "Pub/club_map_2";
            OrigineÉchiquier = new Vector3(0, 4.9f, 0);
           
            Vector3 positionCaméra = Vector3.Zero;
            Vector3 positionObjet = new Vector3(0, 0, 0);
            Vector3 rotationObjet = new Vector3(0, 0, 0);
            

            CouleursÉchiquier = new Color[3];
            CouleursÉchiquier[0] = Color.NavajoWhite;
            CouleursÉchiquier[1] = Color.Gray;
            CouleursÉchiquier[2] = Color.Aquamarine;
           
            

            GestionnaireDeFonts = new RessourcesManager<SpriteFont>(this, "Fonts");
            GestionnaireDeTextures = new RessourcesManager<Texture2D>(this, "Textures");
            GestionnaireDeModèles = new RessourcesManager<Model>(this, "Models");
            GestionnaireDeShaders = new RessourcesManager<Effect>(this, "Effects");
            GestionInput = new InputManager(this);
            GestionSprites = new SpriteBatch(GraphicsDevice);
            CaméraJeu = new CaméraSubjective(this, new Vector3(8.95f,16.12f,-9.59f), new Vector3(8.9f, 15.5f, -8.81f), new Vector3(0.03888775f,0.7823958f,0.6215662f), INTERVALLE_MAJ_STANDARD);

            Services.AddService(typeof(Random), new Random());
            Services.AddService(typeof(RessourcesManager<SpriteFont>), GestionnaireDeFonts);
            Services.AddService(typeof(RessourcesManager<Texture2D>), GestionnaireDeTextures);
            Services.AddService(typeof(RessourcesManager<Model>), GestionnaireDeModèles);
            Services.AddService(typeof(RessourcesManager<Effect>), GestionnaireDeShaders);
            Services.AddService(typeof(InputManager), GestionInput);
            Services.AddService(typeof(Caméra), CaméraJeu);
            Services.AddService(typeof(SpriteBatch), GestionSprites);

            
            CréerMP();
            
            Components.Add(new AfficheurFps(this,"Arial", Color.Blue, INTERVALLE_CALCUL_FPS)) ;
            Components.Add(GestionInput);
            Components.Add(unAfficheur3D  = new Afficheur3D(this));
            Components.Add(PartiEnCours = new Partie(this, TempsDePartie, NomMap, CouleursÉchiquier, OrigineÉchiquier));
            //unAfficheur3D.Visible = false;
            
            
           
            
            

           

            

            base.Initialize();
           
            
        }
        

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
           
            base.LoadContent();
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
            if (GestionInput.EstEnfoncée(Keys.Escape))
            {
                this.Exit();
            }
            if (EstVisible == false)
            {
                if (Bouton2.Clicked == true)
                {
                    unAfficheur3D.Visible = true;
                    Components.Add(CaméraJeu);
                    EstVisible = true;
                    AfficherVoilerMP();
                }
                if (Bouton1.Clicked == true)
                {
                    
                }
                if (Bouton3.Clicked == true && ListeDesBoutons.All(x => x.Visible == true))
                {
                    AfficherVoilerMP();
                    CréerOptions();
                }

            }
            
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }

        void CréerMP()
        {
            float valeur = GraphicsDevice.Viewport.Height / 3 ;
            float y = 0;
            float x = GraphicsDevice.Viewport.Width / 4;
            float longueur = GraphicsDevice.Viewport.Width / 5;
            float hauteur = GraphicsDevice.Viewport.Width / 8;
            Components.Add(Bouton1 = new Bouton(this,"Granite", "Arial", text1, new Vector2(x, y), new Vector2(2*longueur, hauteur)));
            y += valeur;
            Components.Add(Bouton2 = new Bouton(this, "Granite", "Arial", text2, new Vector2(x, y), new Vector2(2*longueur,hauteur)));
            y += valeur;
            Components.Add(Bouton3 = new Bouton(this, "Granite", "Arial", text3, new Vector2(x, y), new Vector2(longueur,hauteur)));
            ListeDesBoutons.Add(Bouton1);
            ListeDesBoutons.Add(Bouton2);
            ListeDesBoutons.Add(Bouton3);
        }
        void CréerOptions()
        {
            //Map
            const float NBR_VERTICALE = 4;
            const float NBR_HORI = 3;
            const int PCT_BOUTONS = 25;
            const int PCT_BORDURES = 10;
            const int PCT_TEXTES = 15; 
            //const int facteurDivision = (int)NBR_HORI - 1;
            float valeurIni = GraphicsDevice.Viewport.Width * (float)(PCT_TEXTES / 100);
            Vector2 bordures = new Vector2(GraphicsDevice.Viewport.Width *(float)(PCT_BORDURES/100), GraphicsDevice.Viewport.Height *(float)(PCT_BORDURES/100) );
            Vector2 dimensions = new Vector2((GraphicsDevice.Viewport.Width - valeurIni - bordures.X)*(float)(PCT_BOUTONS/100),
                GraphicsDevice.Viewport.Height/10);
            float DisEntreBoutons = (GraphicsDevice.Viewport.Width - valeurIni -bordures.X)/8; 
            
           // float valeurX = (GraphicsDevice.Viewport.Width - bordures.X - valeurIni)/6 ;
            //float valeurY = (GraphicsDevice.Viewport.Height - bordures.Y)/NBR_VERTICALE;
           
            float x = bordures.X/2;
            float y = bordures.Y / 2;
            Components.Add(new TexteAffichable(this, "Arial",new Vector2(x,y),"Map:",Color.Black, 0, 2.0f, 0.5f ));
            x +=  valeurIni;
            Components.Add(Bouton4 = new Bouton(this, "Granite", "Arial", text4, new Vector2(x + dimensions.X/2 , y), new Vector2(dimensions.X,dimensions.Y)));
            x += DisEntreBoutons;
            Components.Add(Bouton5 = new Bouton(this, "Granite", "Arial", text5, new Vector2(x + dimensions.X/2, y), new Vector2(dimensions.X, dimensions.Y)));
            x += DisEntreBoutons;
            Components.Add(Bouton6 = new Bouton(this, "Granite", "Arial", text6, new Vector2(x + dimensions.X/2, y), new Vector2(dimensions.X, dimensions.Y)));
            ListeDesBoutons.Add(Bouton4);
            ListeDesBoutons.Add(Bouton5);
            ListeDesBoutons.Add(Bouton6);

        }


        void AfficherVoilerMP()
        {
            for (int i = 0; i < 3; i++)
            {
                ListeDesBoutons.ElementAt(i).Visible = !ListeDesBoutons.ElementAt(i).Visible;
            }
        }
        void AfficherVoilerOPTN()
        {
            for (int i = 3; i < 6; i++)
            {
                ListeDesBoutons.ElementAt(i).Visible = !ListeDesBoutons.ElementAt(i).Visible;
            }
        }
    }
}
