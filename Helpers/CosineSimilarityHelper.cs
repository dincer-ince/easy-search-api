namespace EasySearchApi.Helpers
{
    public static class CosineSimilarityHelper
    {

        public static double CosineSimilarity(double[] doc1, double[] doc2)
        {
            double sum = 0.0;
            double norm1 = 0.0;
            double norm2 = 0.0;

            for(int i=0; i<doc1.Length; i++)
            {
                var a = doc1[i];
                var b = doc2[i];
                sum += a * b;
                norm1 += a * a;
                norm2 += b * b;
            }

            var normalization = Math.Sqrt(norm1* norm2);
            var cosineSimilarity = sum/normalization;

            return cosineSimilarity;
        }
    }
}
