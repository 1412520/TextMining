using Homework1;

namespace Classification
{
    public class Program
    {
        static void Main(string[] args)
        {
            FileUtils.preprocessDirectory("../../training", "../../training/raw_text.txt");
            Bow_tfidf.GenerateTFIDFMatrix("../../training/raw_text.txt", "../../training/processed.txt", "../../training/features.txt", "../../training/tf_idf.txt");
        }
    }
}
