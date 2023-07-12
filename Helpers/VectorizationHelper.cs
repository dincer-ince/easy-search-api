namespace EasySearchApi.Helpers
{
    public static class VectorizationHelper
    {
        public static double[] TfIdfTransform(Document document, List<Word> words)
        {
            List<double> vector = new List<double>();
            int numberOfWords = words.Count;

            foreach(Word word in words)
            {
                if(word.Documents.Where(x=>x.Document == document).Any())
                {
                    var docWord = document.Words.Where(x => x.Word.Equals(word)).First();

                    if(docWord != null)
                    {
                        var tf = docWord.Count;
                        var df = word.Documents.Where(x => x.Document.DictionaryId == document.DictionaryId).Count();
                        var idf = Math.Log((numberOfWords - df +0.5)/(df+0.5));

                        vector.Add(tf*idf);
                    }
                }
                else
                {
                    vector.Add(0.0);
                }
            }

            return vector.ToArray();

        }
    }
}
