using EasySearchApi.Models.DTOs;
using LemmaSharp.Classes;
using Microsoft.ML;

namespace EasySearchApi.Services
{
    public static class PreProcessService
    {
        public static List<string> LemmatizeWords(string[] words)
        {
            var filePath = "./full7z-multext-en.lem";
            var stream = File.OpenRead(filePath);
            var lemmatizer = new Lemmatizer(stream);
            var newWords = new List<string>();
            if(words == null) return new List<string>();
            for(int i = 0; i < words.Length; i++)
            {
                newWords.Add(lemmatizer.Lemmatize(words[i]));
            }
            return newWords;
        }

        public static string[] NormalizeTokenizeStop(Document doc)
        {
            DocumentDTO test = new DocumentDTO()
            {
                text = doc.RawDocument
            };
            var context = new MLContext();
            var emptyData = context.Data.LoadFromEnumerable(new List<DocumentDTO>());

            var embeddingPipeline = context.Transforms.Text.NormalizeText("text", "text", keepDiacritics: true, keepPunctuations: false, keepNumbers: false)
                .Append(context.Transforms.Text.TokenizeIntoWords("tokens", "text"))
                .Append(context.Transforms.Text.RemoveDefaultStopWords("tokens", "tokens", 
                    Microsoft.ML.Transforms.Text.StopWordsRemovingEstimator.Language.English));

            var model = embeddingPipeline.Fit(emptyData);

            var engine = context.Model.CreatePredictionEngine<DocumentDTO, DocumentDTO>(model);

            var text = engine.Predict(test);

            return text.tokens;


        }

    }
}
