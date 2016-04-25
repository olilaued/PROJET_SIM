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
using System.IO;


namespace AtelierXNA
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Jeu : Microsoft.Xna.Framework.Game
    {

        public static bool EstVisible { get; set; }
        public static bool MapChoisie { get; set; }
        const float INTERVALLE_CALCUL_FPS = 1f;
        const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
        float Bordures { get; set; }
        public int Index { get; set; }
        bool MP_VISIBLE = true;
        bool OPTN_VISIBLE = false;
        bool CHOIX_MAP_VISIBLE = false;

        enum GameState { MenuPrincipal, Options, ChoixMaps, ChoixCouleursÉchiquier, ChoixCouleursPièces, ChoixTemps, EnPartie};
        GameState CurrentGameState { get; set; }
        GameState OldGameState { get; set; }
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
        ZoneDéroulante ArrièrePlanDéroulant { get; set; }

        
        

        // Menu principal
        Bouton Bouton1 { get; set; }
        Bouton Bouton2 { get; set; }
        Bouton Bouton3 { get; set; }
        

        string text1 = "Jouer En Solo";
        string text2 = "Jouer 1v1";
        string text3 = "Options";
        
        string text4 = "Map";
        string text5 = "Couleur des pieces";
        string text6 = "Couleurs de l'echiquier";
        string text7 = "Temps de la partie";
        
        string text41 = "Pub";
        string text42= "Parc";

        string text51 = "Classique";
        string text52 = "Vert/Blanc";
        string text53 = "Rouge/Blanc";
        string text54 = "Rose/Blanc";

        


        // Options
        Bouton Bouton4 { get; set; }
        Bouton Bouton5 { get; set; }
        Bouton Bouton6 { get; set; }
        Bouton Bouton7 { get; set; }
        // Maps
        Bouton Bouton41 { get; set; }
        Bouton Bouton42 { get; set; }
        // CouleursÉchiquier
        Bouton Bouton51 { get; set; }
        Bouton Bouton52 { get; set; }
        Bouton Bouton53 { get; set; }
        Bouton Bouton54 { get; set; }

        
       
        
       
       


       

        
        
        



       
        

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

            //NomMap = "Pub/club_map_2";
            //OrigineÉchiquier = new Vector3(163.20f,55.28f,-74.17f);

            NomMap = "Pub/club_map_2";
            OrigineÉchiquier = new Vector3(163.20f,55.28f,-74.17f);
           // OrigineÉchiquier = new Vector3(0, 15, 0);

           
            Vector3 positionObjet = new Vector3(0, 0, 0);
            Vector3 rotationObjet = new Vector3(0, 0, 0);

            //PositionCaméra = new Vector3(171.76f, 65.08f, -68.30f);
            //CibleCaméra = new Vector3(170.96f, 64.49f, -68.35f);
            //OVCaméra = new Vector3(-0.5933704f, 0.8049271f, -0.00201782f);


            CouleursÉchiquier = new Color[3];
           // CouleursÉchiquier[0] = Color.NavajoWhite;
            //CouleursÉchiquier[1] = Color.Gray;
            //CouleursÉchiquier[2] = Color.Aquamarine;
            Components.Add(ArrièrePlanDéroulant = new ZoneDéroulante(this, "chess", new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), INTERVALLE_MAJ_STANDARD));
            //ArrièrePlanDéroulant.Enabled = !ArrièrePlanDéroulant.Enabled;

            GestionnaireDeFonts = new RessourcesManager<SpriteFont>(this, "Fonts");
            GestionnaireDeTextures = new RessourcesManager<Texture2D>(this, "Textures");
            GestionnaireDeModèles = new RessourcesManager<Model>(this, "Models");
            GestionnaireDeShaders = new RessourcesManager<Effect>(this, "Effects");
            GestionInput = new InputManager(this);
            GestionSprites = new SpriteBatch(GraphicsDevice);
            
            Components.Add(new AfficheurFps(this,"Arial", Color.Blue, INTERVALLE_CALCUL_FPS)) ;
            Components.Add(GestionInput);
            Components.Add(unAfficheur3D  = new Afficheur3D(this));
            
           
           

            Services.AddService(typeof(Random), new Random());
            Services.AddService(typeof(RessourcesManager<SpriteFont>), GestionnaireDeFonts);
            Services.AddService(typeof(RessourcesManager<Texture2D>), GestionnaireDeTextures);
            Services.AddService(typeof(RessourcesManager<Model>), GestionnaireDeModèles);
            Services.AddService(typeof(RessourcesManager<Effect>), GestionnaireDeShaders);
            Services.AddService(typeof(InputManager), GestionInput);
            //Services.AddService(typeof(Caméra), CaméraJeu);
            Services.AddService(typeof(SpriteBatch), GestionSprites);

            
            CréerMP();
            CréerOptions();
            CréerChoixMaps();
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
            OldGameState = CurrentGameState;
            switch (CurrentGameState)
            {
                case GameState.MenuPrincipal:
                    if (Bouton3.Clicked == true) { CurrentGameState = GameState.Options; VoilerMP(); AfficherOPTN(); }
                    if (Bouton2.Clicked == true && MapChoisie == true) { CurrentGameState = GameState.EnPartie; VoilerMP();
                    ArrièrePlanDéroulant.Visible = false; CommencerPartie();
                    }
                    
                    
                    break;

                case GameState.Options:
                    if (Bouton4.Clicked == true) { CurrentGameState = GameState.ChoixMaps; VoilerOPTN(); AfficherChoixMaps(); }
                    if (Bouton6.Clicked == true) { CurrentGameState = GameState.ChoixCouleursÉchiquier; VoilerOPTN(); AfficherChoixClrsÉchi(); }
                    if (GestionInput.EstNouvelleTouche(Keys.Escape)) { CurrentGameState = GameState.MenuPrincipal; VoilerOPTN(); AfficherMP();}
                    break;

                case GameState.ChoixMaps:
                    if (Bouton41.Clicked || Bouton42.Clicked == true) { CurrentGameState = GameState.Options; VoilerChoixMaps(); AfficherOPTN(); MapChoisie = true; }
                    break;
                case GameState.ChoixCouleursÉchiquier:
                    if (Bouton51.Clicked || Bouton52.Clicked || Bouton53.Clicked || Bouton54.Clicked == true) { CurrentGameState = GameState.Options; VoilerChoixClrsÉchi();
                        AfficherOPTN(); }
                    break;
                case GameState.EnPartie:
                   
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
           // GraphicsDevice.Clear(Color.White);
            base.Draw(gameTime);
        }
        void CommencerPartie()
        {

            // Components.Add(CaméraJeu = new CaméraSubjective(this, PositionCaméra, CibleCaméra, OVCaméra, INTERVALLE_MAJ_STANDARD));

            Components.Add(CaméraJeu = new CaméraSubjective(this, new Vector3(0,0,0), new Vector3(0,0,0), Vector3.Up,INTERVALLE_MAJ_STANDARD));
            Services.AddService(typeof(Caméra), CaméraJeu);
            unAfficheur3D.Visible = true;
            
            if (NomMap == "Pub/club_map_2")
            {
                Components.Add(PartiEnCours = new Partie(this, TempsDePartie, NomMap, CouleursÉchiquier, OrigineÉchiquier));
            }
            else
            {
                Components.Add(new Terrain(this, 1, new Vector3(0,0,0), new Vector3(0, 0, 0), new Vector3(200, 200, 200), "terrain1", "terrain_textures",3, INTERVALLE_MAJ_STANDARD));
            }
        }

        

        void CréerMP()
        {
            int indice = 3;
            float valeur = GraphicsDevice.Viewport.Height / indice ;
            float y = 0;
            float x = GraphicsDevice.Viewport.Width / 4;
            float longueur = GraphicsDevice.Viewport.Width / 5;
            float hauteur = GraphicsDevice.Viewport.Width / (3*indice) - 1;
            Components.Add(Bouton1 = new Bouton(this, "button", "Arial", text1, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(Bouton2 = new Bouton(this, "button", "Arial", text2, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(Bouton3 = new Bouton(this, "button", "Arial", text3, new Vector2(x, y), new Vector2(longueur, hauteur)));
            ListeDesBoutons.Add(Bouton1); //0
            ListeDesBoutons.Add(Bouton2); //1
            ListeDesBoutons.Add(Bouton3); //2
        }
        void CréerOptions()
        {
            int indice = 4;
            float valeur = GraphicsDevice.Viewport.Height / indice;
            float y = 0;
            float x = GraphicsDevice.Viewport.Width / 2;
            float longueur = GraphicsDevice.Viewport.Width / 5;
            float hauteur = GraphicsDevice.Viewport.Width / (3*indice) - 1;
            Components.Add(Bouton4 = new Bouton(this, "button", "Arial", text4, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(Bouton5 = new Bouton(this, "button", "Arial", text5, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(Bouton6 = new Bouton(this, "button", "Arial", text6, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(Bouton7 = new Bouton(this, "button", "Arial", text7, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            ListeDesBoutons.Add(Bouton4); //3
            ListeDesBoutons.Add(Bouton5); //4
            ListeDesBoutons.Add(Bouton6); //5
            ListeDesBoutons.Add(Bouton7); //6
            VoilerOPTN();
            
        }


        
        void VoilerMP()
        {
            for (int i = 0; i < 3; i++)
            {
                ListeDesBoutons.ElementAt(i).Visible = false;
                ListeDesBoutons.ElementAt(i).Enabled = false;
            }
        }
        void AfficherMP()
        {
            for (int i = 0; i < 3; i++)
            {
                ListeDesBoutons.ElementAt(i).Visible = true;
                ListeDesBoutons.ElementAt(i).Enabled = true;
            }
        }
        
        void VoilerOPTN()
        {
            for (int i = 3; i < 7; i++)
            {
                ListeDesBoutons.ElementAt(i).Visible = false;
                ListeDesBoutons.ElementAt(i).Enabled = false;
            }
        }
        void AfficherOPTN()
        {
            for (int i = 3; i < 7; i++)
            {
                ListeDesBoutons.ElementAt(i).Visible = true;
                ListeDesBoutons.ElementAt(i).Enabled = true;
            }
        }
        void CréerChoixMaps()
        {
            int indice = 2;
            float valeur = GraphicsDevice.Viewport.Height / indice;
            float y = 0;
            float x = GraphicsDevice.Viewport.Width / 2;
            float longueur = GraphicsDevice.Viewport.Width / 5;
            float hauteur = GraphicsDevice.Viewport.Width / (3*indice) - 1;
            Components.Add(Bouton41 = new Bouton(this, "button", "Arial", text41, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(Bouton42 = new Bouton(this, "button", "Arial", text42, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            ListeDesBoutons.Add(Bouton41); //7
            ListeDesBoutons.Add(Bouton42); //8
            VoilerChoixMaps();
        }
       
        
        void VoilerChoixMaps()
        {
            for (int i = 7; i < 9; i++)
            {
                ListeDesBoutons.ElementAt(i).Visible = false;
                ListeDesBoutons.ElementAt(i).Enabled = false;
            }
        }
       void AfficherChoixMaps()
        {
            for (int i = 7; i < 9; i++)
            {
                ListeDesBoutons.ElementAt(i).Visible = true;
                ListeDesBoutons.ElementAt(i).Enabled = true;
            }
        }
        void CréerChoixClrsÉchi()
        {
            int indice = 4;
            float valeur = GraphicsDevice.Viewport.Height / indice;
            float y = 0;
            float x = GraphicsDevice.Viewport.Width / 2;
            float longueur = GraphicsDevice.Viewport.Width / 5;
            float hauteur = GraphicsDevice.Viewport.Width / (3*indice) -1;
            Components.Add(Bouton51 = new Bouton(this, "button", "Arial", text51, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(Bouton52 = new Bouton(this, "button", "Arial", text52, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(Bouton53 = new Bouton(this, "button", "Arial", text53, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(Bouton54 = new Bouton(this, "button", "Arial", text54, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            ListeDesBoutons.Add(Bouton51);
            ListeDesBoutons.Add(Bouton52);
            ListeDesBoutons.Add(Bouton53);
            ListeDesBoutons.Add(Bouton54);
            VoilerChoixClrsÉchi();
 
        }
        void VoilerChoixClrsÉchi()
        {
             for (int i = 9; i < 13; i++)
            {
                ListeDesBoutons.ElementAt(i).Visible = false;
            }
        }
        void AfficherChoixClrsÉchi()
        {
             for (int i = 9; i < 13; i++)
            {
                ListeDesBoutons.ElementAt(i).Visible = true;
            }
        }

        void DéterminerSettings()
        {
            //StreamReader sr = new StreamReader("/../../../../../Settings.txt");
            //Options de la caméra
            int indexMap = ListeDesBoutons.FindIndex(7,2, x => (x.Clicked == true));
            int indexClrÉchi = ListeDesBoutons.FindIndex(9, 3, x => (x.Clicked == true));
            int i = 0;
            int max1;
            
            switch (indexMap)
            {
                case 7: NomMap = "Pub/club_map_2"; OrigineÉchiquier = new Vector3(163.20f, 55.28f, -74.17f); PositionCaméra = new Vector3(171.76f, 65.08f, -68.30f); CibleCaméra = new Vector3(170.96f, 64.49f, -68.35f);
                    OVCaméra = new Vector3(-0.5933704f, 0.8049271f, -0.00201782f); break;
                case 8: NomMap = "Parc"; break;
                    
                    
            }
            //Options Échiquier
            switch (indexClrÉchi)
            {
                case 9: CouleursÉchiquier[0] = Color.White; CouleursÉchiquier[1] = Color.Gray; CouleursÉchiquier[2] = Color.Black; break;
                case 10: CouleursÉchiquier[0] = Color.White; CouleursÉchiquier[1] = Color.Green; CouleursÉchiquier[2] = Color.Black; break;
                case 11: CouleursÉchiquier[0] = Color.White; CouleursÉchiquier[1] = Color.Red; CouleursÉchiquier[2] = Color.Black; break;
                case 12: CouleursÉchiquier[0] = Color.White; CouleursÉchiquier[1] = Color.Pink; CouleursÉchiquier[2] = Color.Black; break;
            }
            
        }

        Vector3 LireLigneVecteur3(string objet)
        {
            int premiereVirgule = objet.IndexOf(objet, 0, objet.First(b => b == ','));
            float x = float.Parse(objet.Substring(0, premiereVirgule));
            int deuxièmeVirgule = objet.IndexOf(objet, premiereVirgule, objet.First(c => c == ','));
            float y = float.Parse(objet.Substring(premiereVirgule, deuxièmeVirgule));
            float z = float.Parse(objet.Substring(deuxièmeVirgule).Trim());
            return new Vector3(x, y, z);
        }

       
        
       
        
    }

    
}
