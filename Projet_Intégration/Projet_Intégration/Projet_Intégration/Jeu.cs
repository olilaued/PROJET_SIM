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
           // CréerOptions();
            //CréerChoixMaps();
            
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
            if (GestionInput.EstNouvelleTouche(Keys.Escape))
            {
                //if (OPTN_VISIBLE == true)
                //{
                //    AfficherVoilerOPTN();
                //   // AfficherVoilerMP();
                //}
            }
            if (EstVisible == false)
            {
                if (Bouton2.Clicked == true)
                {
                    DéterminerSettings();
                    if (MapChoisie == true)
                    { 
                        Components.Add(PartiEnCours = new Partie(this, TempsDePartie, NomMap, CouleursÉchiquier, OrigineÉchiquier));
                        unAfficheur3D.Visible = true;
                        CaméraJeu = new CaméraSubjective(this,PositionCaméra,CibleCaméra,OVCaméra, INTERVALLE_MAJ_STANDARD);
                        Components.Add(CaméraJeu);
                        Services.AddService(typeof(Caméra), CaméraJeu);
                        //CaméraJeu = new CaméraSubjective(this, PositionCaméra, CibleCaméra, OVCaméra, INTERVALLE_MAJ_STANDARD);
                        

                        EstVisible = true;
                        AfficherVoilerMP();
                    }
                    else
                    {

                    }
                }
                if (Bouton1.Clicked == true)
                {
                    
                }
                if (Bouton3.Clicked == true || MP_VISIBLE == false)  //&& ListeDesBoutons.All(x => x.Visible == true))
                {
                    if (MP_VISIBLE == true)
                    {
                        AfficherVoilerMP();
                        //AfficherVoilerOPTN();
                        CréerOptions();
                        MP_VISIBLE = false;
                        OPTN_VISIBLE = true;
                    }
                    if(GestionInput.EstNouvelleTouche(Keys.Escape))
                    {
                        AfficherVoilerMP();
                        AfficherVoilerOPTN();
                        MP_VISIBLE = true;
                        OPTN_VISIBLE = false;
                        CHOIX_MAP_VISIBLE = false;
                      //  return;
                    }
                    if((Bouton4.Clicked == true || CHOIX_MAP_VISIBLE == true))
                    {
                        
                        if (OPTN_VISIBLE == true && CHOIX_MAP_VISIBLE == false)
                        {
                            AfficherVoilerOPTN();
                           // AfficherVoilerChoixMaps();
                            CréerChoixMaps();
                            OPTN_VISIBLE = false;
                            CHOIX_MAP_VISIBLE = true;
                        }
                        

                        if (Bouton41.Clicked == true || Bouton42.Clicked == true)
                        {
                            AfficherVoilerChoixMaps();
                            AfficherVoilerOPTN();
                            MapChoisie = true;
                            OPTN_VISIBLE = true;
                            CHOIX_MAP_VISIBLE = false;
                            
                        }
                    }
                }

                     

                       
                    
                    //if (Bouton5.Clicked == true)
                    //{

                    //}
                

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
            ListeDesBoutons.Add(Bouton1); //0
            ListeDesBoutons.Add(Bouton2); //1
            ListeDesBoutons.Add(Bouton3); //2
        }
        void CréerOptions()
        {

            float valeur = GraphicsDevice.Viewport.Height / 4;
            float y = 0;
            float x = GraphicsDevice.Viewport.Width / 2;
            float longueur = GraphicsDevice.Viewport.Width / 5;
            float hauteur = GraphicsDevice.Viewport.Width / 8;
            Components.Add(Bouton4 = new Bouton(this, "Granite", "Arial", text4, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(Bouton5 = new Bouton(this, "Granite", "Arial", text5, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(Bouton6 = new Bouton(this, "Granite", "Arial", text6, new Vector2(x, y), new Vector2(2* longueur, hauteur)));
            y += valeur;
            Components.Add(Bouton7 = new Bouton(this, "Granite", "Arial", text7, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            ListeDesBoutons.Add(Bouton4); //3
            ListeDesBoutons.Add(Bouton5); //4
            ListeDesBoutons.Add(Bouton6); //5
            ListeDesBoutons.Add(Bouton7); //6
            //OPTN_VISIBLE = true;
           // AfficherVoilerOPTN();
        }


        void AfficherVoilerMP()
        {
            for (int i = 0; i < 3; i++)
            {
                ListeDesBoutons.ElementAt(i).Visible = !ListeDesBoutons.ElementAt(i).Visible;
                ListeDesBoutons.ElementAt(i).Enabled = !ListeDesBoutons.ElementAt(i).Enabled;
                
            }
            MP_VISIBLE = !MP_VISIBLE;
        }
        void AfficherVoilerOPTN()
        {
            for (int i = 3; i < 7; i++)
            {
                ListeDesBoutons.ElementAt(i).Visible = !ListeDesBoutons.ElementAt(i).Visible;
                ListeDesBoutons.ElementAt(i).Enabled = !ListeDesBoutons.ElementAt(i).Enabled;
            }
            //OPTN_VISIBLE = !OPTN_VISIBLE;
        }
        void CréerChoixMaps()
        {
            float valeur = GraphicsDevice.Viewport.Height / 2;
            float y = 0;
            float x = GraphicsDevice.Viewport.Width / 2;
            float longueur = GraphicsDevice.Viewport.Width / 5;
            float hauteur = GraphicsDevice.Viewport.Width / 8;
            Components.Add(Bouton41 = new Bouton(this, "Granite", "Arial", text41, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(Bouton42 = new Bouton(this, "Granite", "Arial", text42, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            ListeDesBoutons.Add(Bouton41); //7
            ListeDesBoutons.Add(Bouton42); //8
           // AfficherVoilerChoixMaps();
        }
        void AfficherVoilerChoixMaps()
        {
            for (int i = 7; i < 9; i++)
            {
                ListeDesBoutons.ElementAt(i).Visible = !ListeDesBoutons.ElementAt(i).Visible;
                ListeDesBoutons.ElementAt(i).Enabled = !ListeDesBoutons.ElementAt(i).Enabled;
            }
            
        }
        void CréerChoixClrsÉchi()
        {
            float valeur = GraphicsDevice.Viewport.Height / 4;
            float y = 0;
            float x = GraphicsDevice.Viewport.Width / 2;
            float longueur = GraphicsDevice.Viewport.Width / 5;
            float hauteur = GraphicsDevice.Viewport.Width / 8;
            Components.Add(Bouton51 = new Bouton(this, "Granite", "Arial", text51, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(Bouton52 = new Bouton(this, "Granite", "Arial", text52, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(Bouton53 = new Bouton(this, "Granite", "Arial", text53, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            y += valeur;
            Components.Add(Bouton54 = new Bouton(this, "Granite", "Arial", text54, new Vector2(x, y), new Vector2(2 * longueur, hauteur)));
            ListeDesBoutons.Add(Bouton51);
            ListeDesBoutons.Add(Bouton52);
            ListeDesBoutons.Add(Bouton53);
            ListeDesBoutons.Add(Bouton54);
            AfficherVoilerChoixClrsÉchi();
 
        }
        void AfficherVoilerChoixClrsÉchi()
        {
            for (int i = 9; i < 13; i++)
            {
                ListeDesBoutons.ElementAt(i).Visible = !ListeDesBoutons.ElementAt(i).Visible;
            }
        }

        void DéterminerSettings()
        {
            //StreamReader sr = new StreamReader("/../../../../../Settings.txt");
            //Options de la caméra
            int indexMap = ListeDesBoutons.FindIndex(7,2, x => (x.Clicked == true));
            //int indexClrÉchi = ListeDesBoutons.FindIndex(9, 3, x => (x.Clicked == true));
            int i = 0;
            int max1;
            //switch (indexMap)
            //{
            //    case 7: max1 = 2; NomMap = "Pub"; OrigineÉchiquier = new Vector3(163.20f, 55.28f, -74.17f);break;
            //    case 8:max1 = 6;NomMap = "Parc";break;
            //    default:max1 = 0;break;
            //}
            //if (max1 > 0)
            //{
            //    while (i < max1)
            //    {
            //        sr.ReadLine();
            //    }
            //    string position = sr.ReadLine();
            //    PositionCaméra = LireLigneVecteur3(position);
            //    string cible = sr.ReadLine();
            //    CibleCaméra = LireLigneVecteur3(cible);
            //    string oV = sr.ReadLine();
            //    OVCaméra = LireLigneVecteur3(oV);
            //}
            switch (indexMap)
            {
                case 7: NomMap = "Pub"; OrigineÉchiquier = new Vector3(163.20f, 55.28f, -74.17f); PositionCaméra = new Vector3(171.76f, 65.08f, -68.30f); CibleCaméra = new Vector3(170.96f, 64.49f, -68.35f);
                    OVCaméra = new Vector3(-0.5933704f, 0.8049271f, -0.00201782f); break;
                case 8: NomMap = "Parc"; break;
                    
                    
            }
            //Options Échiquier
            //switch (indexClrÉchi)
            //{
            //    case 9: CouleursÉchiquier[0] = Color.White ; CouleursÉchiquier[1] = Color.Gray; CouleursÉchiquier[2] = Color.Black;break;
            //    case 10: CouleursÉchiquier[0] = Color.White ; CouleursÉchiquier[1] = Color.Green; CouleursÉchiquier[2] = Color.Black;break;
            //    case 11:CouleursÉchiquier[0] = Color.White ; CouleursÉchiquier[1] = Color.Red; CouleursÉchiquier[2] = Color.Black;break;
            //    case 12:CouleursÉchiquier[0] = Color.White ; CouleursÉchiquier[1] = Color.Pink; CouleursÉchiquier[2] = Color.Black;break;
            //}
            
            




            //int premiereVirgule = position.IndexOf(position,0,position.First(b => b == ','));
            //float x = float.Parse(position.Substring(0, premiereVirgule));
            //int deuxièmeVirgule = position.IndexOf(position,premiereVirgule,position.First(c => c == ','));
            //float y = float.Parse(position.Substring(premiereVirgule,deuxièmeVirgule));
            //float z = float.Parse(position.Substring(deuxièmeVirgule).Trim());
            //PositionCaméra = new Vector3(x, y, z);

            
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
