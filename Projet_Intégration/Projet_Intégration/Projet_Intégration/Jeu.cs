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
        public int Index { get; set; }
        
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
        Vector3 PositionCaméra { get; set; }
        Vector3 CibleCaméra { get; set; }
        Vector3 OVCaméra { get; set; }
        enum GameState { MenuPrincipal, Options,ClrsPièces, ClrsÉchiquier, Temps}
        GameState CurrentGameState { get; set; }
        ZoneDéroulante ArrièrePlanDéroulant { get; set; }
        public Vector3 CentreBoard { get; set; }
        
        
        // Menu principal
        Bouton Bouton1 { get; set; }
        Bouton Bouton2 { get; set; }

        string text1 = "Jouer 1v1";
        string text2 = "Options";


        string text3 = "Couleur des pieces";
        string text4 = "Couleurs de l'echiquier";
        string text5 = "Temps de la partie";


        string text41 = "Classique";
        string text42 = "Vert/Blanc";
        string text43 = "Rouge/Blanc";
        string text44 = "Rose/Blanc";
        


        // Options
        Bouton Bouton3 { get; set; }
        Bouton Bouton4 { get; set; }
        Bouton Bouton5 { get; set; }


        // CouleursÉchiquier
        Bouton Bouton41 { get; set; }
        Bouton Bouton42 { get; set; }
        Bouton Bouton43 { get; set; }
        Bouton Bouton44 { get; set; }
        
       
       


       

        
        
        



       
        

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
            CurrentGameState = GameState.MenuPrincipal;
            ListeDesBoutons = new List<Bouton>();
            TempsDePartie = 15 * 60;
            NomMap = "Pub/club_map_2";
            OrigineÉchiquier = new Vector3(163.20f,55.28f,-74.17f);
           // OrigineÉchiquier = new Vector3(0, 15, 0);
           
            Vector3 positionObjet = new Vector3(0, 0, 0);
            Vector3 rotationObjet = new Vector3(0, 0, 0);

            PositionCaméra = new Vector3(140.76f, 65.08f, -68.30f);
            //CibleCaméra = new Vector3(170.96f, 64.49f, -68.35f);
            CibleCaméra = new Vector3(157.2f,55.28f,-68.17f);

            OVCaméra = new Vector3(-0.5933704f, 0.8049271f, -0.00201782f);


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
 
            Components.Add(ArrièrePlanDéroulant = new ZoneDéroulante(this, "chess", new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), INTERVALLE_MAJ_STANDARD));
            Components.Add(new AfficheurFps(this,"Arial", Color.Blue, INTERVALLE_CALCUL_FPS)) ;
            Components.Add(GestionInput);
            Components.Add(unAfficheur3D  = new Afficheur3D(this));
            CaméraJeu = new CaméraSubjective(this,PositionCaméra,CibleCaméra,OVCaméra, INTERVALLE_MAJ_STANDARD);
            Components.Add(PartiEnCours = new Partie(this, TempsDePartie, NomMap, CouleursÉchiquier, OrigineÉchiquier));
           

            Services.AddService(typeof(Random), new Random());
            Services.AddService(typeof(RessourcesManager<SpriteFont>), GestionnaireDeFonts);
            Services.AddService(typeof(RessourcesManager<Texture2D>), GestionnaireDeTextures);
            Services.AddService(typeof(RessourcesManager<Model>), GestionnaireDeModèles);
            Services.AddService(typeof(RessourcesManager<Effect>), GestionnaireDeShaders);
            Services.AddService(typeof(InputManager), GestionInput);
            Services.AddService(typeof(Caméra), CaméraJeu);
            Services.AddService(typeof(SpriteBatch), GestionSprites);

            
            CréerMP();
            CréerOptions();
            CréerChoixClrsÉchi();
            Components.Add(CaméraJeu);
            
            
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
            switch (CurrentGameState)
            {
                case GameState.MenuPrincipal:
                    
                    if (Bouton1.Clicked == true)
                    {
                        unAfficheur3D.Visible = true;
                     //   Components.Add(CaméraJeu);
                        ArrièrePlanDéroulant.ModifierActivation();
                        EstVisible = true;
                        AfficherVoilerMP();
                    }

                    if (Bouton2.Clicked == true)
                    {
                        CurrentGameState = GameState.Options;
                        VoilerMP();
                        AfficherOPTN();
                    }
                    break;
                case GameState.Options:

                    if (Bouton4.Clicked == true)
                    {
                        CurrentGameState = GameState.ClrsÉchiquier;
                        VoilerOPTN();
                        AfficherChoixClrsÉchi();
                    }
                    if (GestionInput.EstNouvelleTouche(Keys.Escape))
                    {
                        CurrentGameState = GameState.MenuPrincipal;
                        VoilerOPTN();
                        AfficherMP();
                    }
                    break;
                case GameState.ClrsÉchiquier:
                    if (Bouton41.Clicked || Bouton42.Clicked || Bouton43.Clicked || Bouton44.Clicked == true)
                    {
                        CurrentGameState = GameState.Options;
                        VoilerChoixClrsÉchi();
                        AfficherOPTN();

                    }

                    break;


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
            int indice = 2;
            float valeur = GraphicsDevice.Viewport.Height / indice;
            float y = 0;
            float x = GraphicsDevice.Viewport.Width / 4;
            float longueur = GraphicsDevice.Viewport.Width / 5;
            float hauteur = GraphicsDevice.Viewport.Width / (3 * indice) - 1;
            Components.Add(Bouton1 = new Bouton(this, "button", "Arial", text1, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(Bouton2 = new Bouton(this, "button", "Arial", text2, new Vector2(x, y), new Vector2(longueur, hauteur)));
            ListeDesBoutons.Add(Bouton1); //0
            ListeDesBoutons.Add(Bouton2); //1
        }

        void CréerOptions()
        {
            int indice = 3;
            float valeur = GraphicsDevice.Viewport.Height / indice;
            float y = 0;
            float x = GraphicsDevice.Viewport.Width / 2;
            float longueur = GraphicsDevice.Viewport.Width / 5;
            float hauteur = GraphicsDevice.Viewport.Width / (3 * indice) - 1;
            Components.Add(Bouton3 = new Bouton(this, "button", "Arial", text3, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(Bouton4 = new Bouton(this, "button", "Arial", text4, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(Bouton5 = new Bouton(this, "button", "Arial", text5, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            ListeDesBoutons.Add(Bouton3); //2
            ListeDesBoutons.Add(Bouton4); //3
            ListeDesBoutons.Add(Bouton5); //4
            VoilerOPTN();

        }



        void VoilerMP()
        {
            for (int i = 0; i < 2; i++)
            {
                ListeDesBoutons.ElementAt(i).Visible = false;
                ListeDesBoutons.ElementAt(i).Enabled = false;
            }
        }
        void AfficherMP()
        {
            for (int i = 0; i < 2; i++)
            {
                ListeDesBoutons.ElementAt(i).Visible = true;
                ListeDesBoutons.ElementAt(i).Enabled = true;
            }
        }

        void VoilerOPTN()
        {
            for (int i = 2; i < 5; i++)
            {
                ListeDesBoutons.ElementAt(i).Visible = false;
                ListeDesBoutons.ElementAt(i).Enabled = false;
            }
        }
        void AfficherOPTN()
        {
            for (int i = 2; i < 5; i++)
            {
                ListeDesBoutons.ElementAt(i).Visible = true;
                ListeDesBoutons.ElementAt(i).Enabled = true;
            }
        }


        void AfficherVoilerMP()
        {
            for (int i = 0; i < 2; i++)
            {
                ListeDesBoutons.ElementAt(i).Visible = !ListeDesBoutons.ElementAt(i).Visible;
            }
        }
        void AfficherVoilerOPTN()
        {
            for (int i = 2; i < 5; i++)
            {
                ListeDesBoutons.ElementAt(i).Visible = !ListeDesBoutons.ElementAt(i).Visible;
            }
        }
        void CréerChoixClrsÉchi()
        {
            int indice = 4;
            float valeur = GraphicsDevice.Viewport.Height / indice;
            float y = 0;
            float x = GraphicsDevice.Viewport.Width / 2;
            float longueur = GraphicsDevice.Viewport.Width / 5;
            float hauteur = GraphicsDevice.Viewport.Width / (3 * indice) - 1;
            Components.Add(Bouton41 = new Bouton(this, "button", "Arial", text41, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(Bouton42 = new Bouton(this, "button", "Arial", text42, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(Bouton43 = new Bouton(this, "button", "Arial", text43, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(Bouton44 = new Bouton(this, "button", "Arial", text44, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            ListeDesBoutons.Add(Bouton41); //5
            ListeDesBoutons.Add(Bouton42); //6
            ListeDesBoutons.Add(Bouton43); //7 
            ListeDesBoutons.Add(Bouton44); //8
            VoilerChoixClrsÉchi();

        }
        void VoilerChoixClrsÉchi()
        {
            for (int i = 5; i < 9; i++)
            {
                ListeDesBoutons.ElementAt(i).Visible = false;
            }
        }
        void AfficherChoixClrsÉchi()
        {
            for (int i = 5; i < 9; i++)
            {
                ListeDesBoutons.ElementAt(i).Visible = true;
            }
        }
        void DéterminerSettings()
        {
            //StreamReader sr = new StreamReader("/../../../../../Settings.txt");
            //Options de la caméra
            int indexMap = ListeDesBoutons.FindIndex(7, 2, x => (x.Clicked == true));
            int indexClrÉchi = ListeDesBoutons.FindIndex(5, 3, x => (x.Clicked == true));


            //Options Échiquier
            switch (indexClrÉchi)
            {
                case 9: CouleursÉchiquier[0] = Color.White; CouleursÉchiquier[1] = Color.Gray; CouleursÉchiquier[2] = Color.Black; break;
                case 10: CouleursÉchiquier[0] = Color.White; CouleursÉchiquier[1] = Color.Green; CouleursÉchiquier[2] = Color.Black; break;
                case 11: CouleursÉchiquier[0] = Color.White; CouleursÉchiquier[1] = Color.Red; CouleursÉchiquier[2] = Color.Black; break;
                case 12: CouleursÉchiquier[0] = Color.White; CouleursÉchiquier[1] = Color.Pink; CouleursÉchiquier[2] = Color.Black; break;
            }

        }
    }
}
