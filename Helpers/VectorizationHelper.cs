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
                if(word.documents.Where(x=>x.document == document).Any())
                {
                    var docWord = document.words.Where(x => x.word.Equals(word)).First();

                    if(docWord != null)
                    {
                        var tf = docWord.count;
                        var df = word.documents.Where(x => x.document.dictionaryId == document.dictionaryId).Count();
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
