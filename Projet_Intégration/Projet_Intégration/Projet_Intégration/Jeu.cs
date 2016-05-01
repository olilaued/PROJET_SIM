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
        const float PROFONDEUR_DEFAUT = 0.5f;
        const string VAINQUEUR_N = "LES NOIRS L'EMPORTENT!";
        const string VAINQUEUR_B = "LES BLANCS L'EMPORTENT!";
        
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
        AfficheurFps UnAfficheurFPS { get; set; }
        Color[] CouleursÉchiquier { get; set; }
        Partie PartiEnCours { get; set; }
        Afficheur3D unAfficheur3D { get; set; }
        public static List<Bouton> ListeDesBoutons { get; protected set; }
        string NomMap { get; set; }
        float TempsDePartie { get; set; }
        Vector3 OrigineÉchiquier { get; set; }
        Vector3 PositionCaméra { get; set; }
        Vector3 CibleCaméra { get; set; }
        Vector3 OVCaméra { get; set; }
        public enum GameState { MenuPrincipal, Options,Musique, ClrsÉchiquier, TempsPartie, EnJeu, EnPause}
       public static GameState CurrentGameState { get; set; }
        ZoneDéroulante ArrièrePlanDéroulant { get; set; }
        TexteAffichable GagnantN { get; set; }
        TexteAffichable GagnantB { get; set; }
         
        
        // Menu principal
        string text1 = "Jouer 1v1";
        string text2 = "Options";
        Bouton B1;
        Bouton B2;


        // Options
        string text3 = "Couleurs de l'echiquier";
        string text4 = "Temps de la partie";
        string text5 = "Musique d'ambiance";
        Bouton B3;
        Bouton B4;
        Bouton B5;


        //Pause
        string msgPause1 = "Reprendre la partie";
        string msgPause2 = "Quitter la partie";
        Bouton B6;
        Bouton B7;

        // CouleursÉchiquier
        
        string clr1 = "Classique";
        string clr2 = "Vert/Blanc";
        string clr3 = "Rouge/Blanc";
        string clr4 = "Rose/Blanc";
        Bouton B8;
        Bouton B9;
        Bouton B10;
        Bouton B11;
        //ChoixTemps
        string temps1 = "15 minutes";
        string temps2 = "30 minutes";
        string temps3 = "45 minutes";
        string temps4 = "60 minutes";
        Bouton B12;
        Bouton B13;
        Bouton B14;
        Bouton B15;


       

       
        
       
       


       

        
        
        



       
        

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
            TempsDePartie = 15*60 ;
            NomMap = "Pub/club_map_2";
            OrigineÉchiquier = new Vector3(163.20f,55.28f,-74.17f);
           // OrigineÉchiquier = new Vector3(0, 15, 0);
           
            Vector3 positionObjet = new Vector3(0, 0, 0);
            Vector3 rotationObjet = new Vector3(0, 0, 0);

            PositionCaméra = new Vector3(140.76f, 65.08f, -68.30f);
            CibleCaméra = new Vector3(157.2f, 55.28f, -68.17f);
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
            Components.Add(UnAfficheurFPS = new AfficheurFps(this,"Arial", Color.Blue, INTERVALLE_CALCUL_FPS)) ;
            Components.Add(GestionInput);
            Components.Add(unAfficheur3D  = new Afficheur3D(this));
            CaméraJeu = new CaméraSubjective(this,PositionCaméra,CibleCaméra,OVCaméra, INTERVALLE_MAJ_STANDARD);
           // Components.Add(PartiEnCours = new Partie(this, TempsDePartie, NomMap, CouleursÉchiquier, OrigineÉchiquier));
           

            Services.AddService(typeof(Random), new Random());
            Services.AddService(typeof(RessourcesManager<SpriteFont>), GestionnaireDeFonts);
            Services.AddService(typeof(RessourcesManager<Texture2D>), GestionnaireDeTextures);
            Services.AddService(typeof(RessourcesManager<Model>), GestionnaireDeModèles);
            Services.AddService(typeof(RessourcesManager<Effect>), GestionnaireDeShaders);
            Services.AddService(typeof(InputManager), GestionInput);
            Services.AddService(typeof(Caméra), CaméraJeu);
            Services.AddService(typeof(SpriteBatch), GestionSprites);


            //CréerInterface("MenuPrincipal"); // 0, 1
            //CréerInterface("Options"); // 2,3,4
            //CréerInterface("MenuPause"); // 5,6,
            //CréerInterface("ClrsÉchiquier"); //7,8,9,10
            //CréerInterface("TempsPartie"); // 11,12,13,14
            //CréerInterface("Musique"); //15,16
            //VoilerBoutons(2, 14);
            CréerMP();
            CréerOptions();
            CréerChoixPause();
            CréerChoixClrsÉchi();
            
            
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
                    
                    if (B1.Clicked == true) 
                    { 
                        DéterminerSettings();
                        CommencerPartie();
                        VoilerBoutons(0, 2);
                        CurrentGameState = GameState.EnJeu;
                    }

                    if (B2.Clicked == true)
                    {
                        CurrentGameState = GameState.Options;
                        VoilerBoutons(0, 2);
                        AfficherBoutons(2, 5);
                    }
                    break;

                case GameState.Options:

                    if (B3.Clicked == true)
                    {
                        CurrentGameState = GameState.ClrsÉchiquier;
                        VoilerBoutons(2, 5);
                        AfficherBoutons(7, 11);
                    }
                    if (GestionInput.EstNouvelleTouche(Keys.Escape))
                    {
                        CurrentGameState = GameState.MenuPrincipal;
                        VoilerBoutons(2, 5);
                        AfficherBoutons(0, 2);
                    }
                    break;

                case GameState.ClrsÉchiquier:
                    if (B8.Clicked  || B9.Clicked || B10.Clicked == true || B11.Clicked ==true)
                    {
                        CurrentGameState = GameState.Options;
                        VoilerBoutons(7,11);
                        AfficherBoutons(2,5);
                    }
                    break;
                case GameState.EnJeu:
                    if (PartiEnCours.PartieTerminée)
                    {
                        if (PartiEnCours.TourActuel.Couleur == "White")
                        {
                            Components.Add(GagnantN = new TexteAffichable(this, "Arial", VAINQUEUR_N, Color.LightGreen, 0, 3.0f, PROFONDEUR_DEFAUT));
                            
                        }
                        else
                        {
                            Components.Add(GagnantB = new TexteAffichable(this, "Arial", VAINQUEUR_B, Color.LightGreen, 0, 3.0f, PROFONDEUR_DEFAUT));
                        }
                        Components.Remove(PartiEnCours.TourActuel);
                    }
                    if (GestionInput.EstNouvelleTouche(Keys.Escape))
                    {
                        CurrentGameState = GameState.EnPause;
                        AfficherBoutons(5, 7);
                    }
                       
 
                    
                    break;

            }
          
            
            base.Update(gameTime);
        }
        void CommencerPartie()
        {
            //Components.Add(unAfficheur3D);
            unAfficheur3D.Visible = true;
            Components.Add(CaméraJeu);
            ArrièrePlanDéroulant.ModifierActivation();
            Components.Add(PartiEnCours = new Partie(this, TempsDePartie, NomMap, CouleursÉchiquier, OrigineÉchiquier));
            //Components.Add(UnAfficheurFPS);
            //Components.Add(UnAfficheurFPS);
            
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
            Components.Add(B1 = new Bouton(this, "button", "Arial", text1, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(B2 = new Bouton(this, "button", "Arial", text2, new Vector2(x, y), new Vector2(longueur, hauteur)));
            ListeDesBoutons.Add(B1); //0
            ListeDesBoutons.Add(B2); //1
        }

        void CréerOptions()
        {
            int indice = 3;
            float valeur = GraphicsDevice.Viewport.Height / indice;
            float y = 0;
            float x = GraphicsDevice.Viewport.Width / 2;
            float longueur = GraphicsDevice.Viewport.Width / 5;
            float hauteur = GraphicsDevice.Viewport.Width / (3 * indice) - 1;
            Components.Add(B3 = new Bouton(this, "button", "Arial", text3, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(B4 = new Bouton(this, "button", "Arial", text4, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(B5 = new Bouton(this, "button", "Arial", text5, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            ListeDesBoutons.Add(B3); //2
            ListeDesBoutons.Add(B4); //3
            ListeDesBoutons.Add(B5); //4
            VoilerBoutons(2, 5);

        }




        void CréerChoixClrsÉchi()
        {
            int indice = 4;
            float valeur = GraphicsDevice.Viewport.Height / indice;
            float y = 0;
            float x = GraphicsDevice.Viewport.Width / 2;
            float longueur = GraphicsDevice.Viewport.Width / 5;
            float hauteur = GraphicsDevice.Viewport.Width / (3 * indice) - 1;
            Components.Add(B8 = new Bouton(this, "button", "Arial", clr1, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(B9 = new Bouton(this, "button", "Arial", clr2, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(B10 = new Bouton(this, "button", "Arial", clr3, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(B11 = new Bouton(this, "button", "Arial", clr4, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            ListeDesBoutons.Add(B8); //5
            ListeDesBoutons.Add(B9); //6
            ListeDesBoutons.Add(B10); //7 
            ListeDesBoutons.Add(B11); //8
            VoilerBoutons(7, 11);

        }

        void CréerChoixPause()
        {
            int indice = 2;
            float valeur = GraphicsDevice.Viewport.Height / indice;
            float y = 0;
            float x = GraphicsDevice.Viewport.Width / 2;
            float longueur = GraphicsDevice.Viewport.Width / 5;
            float hauteur = GraphicsDevice.Viewport.Width / (3 * indice) - 1;
            Components.Add(B6 = new Bouton(this, "button", "Arial", msgPause1, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(B7 = new Bouton(this, "button", "Arial", msgPause2, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
           
            ListeDesBoutons.Add(B6); //7 
            ListeDesBoutons.Add(B7); //8
            VoilerBoutons(5,7);

        }
        void DéterminerSettings()
        {
            //StreamReader sr = new StreamReader("/../../../../../Settings.txt");
            //Options de la caméra
            int indexClrÉchi = ListeDesBoutons.FindIndex(7, 4, x => (x.Clicked == true));
            //int indexTemps = ListeDesBoutons.FindIndex();

            //Options Échiquier
            switch (indexClrÉchi)
            {
                case 7: CouleursÉchiquier[0] = Color.White; CouleursÉchiquier[1] = Color.Gray; CouleursÉchiquier[2] = Color.Black; break;
                case 8: CouleursÉchiquier[0] = Color.White; CouleursÉchiquier[1] = Color.Green; CouleursÉchiquier[2] = Color.Black; break;
                case 9: CouleursÉchiquier[0] = Color.White; CouleursÉchiquier[1] = Color.Red; CouleursÉchiquier[2] = Color.Black; break;
                case 10: CouleursÉchiquier[0] = Color.White; CouleursÉchiquier[1] = Color.Pink; CouleursÉchiquier[2] = Color.Black; break;
            }

        }
        
        void AfficherBoutons(int indiceA, int indiceB)
        {
            for (int i = indiceA; i < indiceB; i++)
            {
                ListeDesBoutons.ElementAt(i).Visible = true;
                ListeDesBoutons.ElementAt(i).Enabled = true;
            }
        }
        
        void VoilerBoutons(int indiceA, int indiceB)
        {
            for (int i = indiceA; i < indiceB; i++)
            {
                ListeDesBoutons.ElementAt(i).Visible = false;
                ListeDesBoutons.ElementAt(i).Enabled = false;
            }
        }
    
    
        //void CréerInterface(string nomInterface)
        //{
        //    //int nbBoutons = 0;
        //    string message1;
        //    string message2;
        //    string message3;
        //    string message4;
        //    int x = GraphicsDevice.Viewport.Width / 2;
        //    float y = 0;
        //    float longueur = GraphicsDevice.Viewport.Width / 5;
        //    float nbBoutons;
        //    float valeur;
        //    float hauteur;
        //    switch (nomInterface)
        //    {
        //        case "MenuPrincipal":
        //            message1 = text1;
        //            message2 = text2;
        //            nbBoutons = 2;
        //            valeur = GraphicsDevice.Viewport.Height / nbBoutons;
        //            hauteur = GraphicsDevice.Viewport.Height / (3 * nbBoutons) - 1;
                    
        //            Components.Add(B1 = new Bouton(this, "button", "Arial", message1, new Vector2(x/2,y),new Vector2(2 * longueur, hauteur)));
        //            y += valeur;
        //            Components.Add(B2 = new Bouton(this, "button", "Arial", message2, new Vector2(x/2, y), new Vector2( longueur, hauteur)));

        //            ListeDesBoutons.Add(B1);
        //            ListeDesBoutons.Add(B2);
        //            break;
        //        case "Options":
        //            message1 = text3;
        //            message2 = text4;
        //            message3 = text5;
        //            nbBoutons = 3;
        //            valeur = GraphicsDevice.Viewport.Height / nbBoutons;
        //            hauteur = GraphicsDevice.Viewport.Height / (3 * nbBoutons) - 1;
                    

        //            Components.Add(B3 = new Bouton(this, "button", "Arial", message1, new Vector2(x,y),new Vector2(2 * longueur, hauteur)));
        //            y += valeur;
        //            Components.Add(B4 = new Bouton(this, "button", "Arial", message2, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
        //            y += valeur;
        //            Components.Add(B5 = new Bouton(this, "button", "Arial", message3, new Vector2(x,y),new Vector2(2 * longueur, hauteur)));

        //            ListeDesBoutons.Add(B3);
        //            ListeDesBoutons.Add(B4);
        //            ListeDesBoutons.Add(B5);
        //            break;

        //        case "MenuPause":
        //            message1 = msgPause1;
        //            message2 = msgPause2;
        //            nbBoutons = 2;
        //            valeur = GraphicsDevice.Viewport.Height / nbBoutons;
        //            hauteur = GraphicsDevice.Viewport.Height / (3 * nbBoutons) - 1;

        //            Components.Add(B6 = new Bouton(this, "button", "Arial", message1, new Vector2(x,y),new Vector2(2 * longueur, hauteur)));
        //            y += valeur;
        //            Components.Add(B7 = new Bouton(this, "button", "Arial", message2, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));

        //            ListeDesBoutons.Add(B6);
        //            ListeDesBoutons.Add(B7);
        //            break;

        //        case "ClrsÉchiquier":
        //            message1 = clr1;
        //            message2 = clr2;
        //            message3 = clr3;
        //            message4 = clr4;
        //            nbBoutons = 2;

        //            valeur = GraphicsDevice.Viewport.Height / nbBoutons;
        //            hauteur = GraphicsDevice.Viewport.Height / (3 * nbBoutons) - 1;
                    

        //            Components.Add(B8 = new Bouton(this, "button", "Arial", message1, new Vector2(x,y),new Vector2(2 * longueur, hauteur)));
        //            y += valeur;
        //            Components.Add(B9 = new Bouton(this, "button", "Arial", message2, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
        //            y += valeur;
        //            Components.Add(B10 = new Bouton(this, "button", "Arial", message3, new Vector2(x,y),new Vector2(2 * longueur, hauteur)));
        //            y += valeur;
        //            Components.Add(B11 = new Bouton(this, "button", "Arial", message4, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
                    
        //            ListeDesBoutons.Add(B8);
        //            ListeDesBoutons.Add(B9);
        //            ListeDesBoutons.Add(B10);
        //            ListeDesBoutons.Add(B11);
        //            break;
        //   case "TempsPartie":
        //            message1 =temps1;
        //            message2 = temps2;
        //            message3 = temps3;
        //            message4 = temps4;
        //            nbBoutons = 2;

        //            valeur = GraphicsDevice.Viewport.Height / nbBoutons;
        //            hauteur = GraphicsDevice.Viewport.Height / (3 * nbBoutons) - 1;
                    

        //            Components.Add(B12 = new Bouton(this, "button", "Arial", message1, new Vector2(x,y),new Vector2(2 * longueur, hauteur)));
        //            y += valeur;
        //            Components.Add(B13 = new Bouton(this, "button", "Arial", message2, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
        //            y += valeur;
        //            Components.Add(B14 = new Bouton(this, "button", "Arial", message3, new Vector2(x,y),new Vector2(2 * longueur, hauteur)));
        //            y += valeur;
        //            Components.Add(B15 = new Bouton(this, "button", "Arial", message4, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
                    
        //            ListeDesBoutons.Add(B12);
        //            ListeDesBoutons.Add(B13);
        //            ListeDesBoutons.Add(B14);
        //            ListeDesBoutons.Add(B15);
        //            break;

        //    }
           
            
        //}
        

       
    }
}
