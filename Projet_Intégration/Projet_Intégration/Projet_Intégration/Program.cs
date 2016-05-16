using System;


namespace AtelierXNA
{
#if WINDOWS || XBOX
    static class Program
    {
        
        
        static void Main(string[] args)
        {
            using (Jeu jeu = new Jeu())
            {
                jeu.Run();
            }
        }
    }
#endif
}

