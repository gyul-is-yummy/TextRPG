using TextRPG.Managers;

namespace TextRPG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MainGame textRPG = new MainGame();
            textRPG.InputName();
            textRPG.SelectJob();
            textRPG.GameStart();
        }
    }
}
