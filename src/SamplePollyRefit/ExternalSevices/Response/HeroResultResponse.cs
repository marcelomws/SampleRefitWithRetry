namespace SamplePollyRefit.ExternalSevices.Response
{
    public class HeroResultResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public BiographyResponse Biography { get; set; }
        public AppearanceResponse Appearance { get; set; }
        public ImageResponse Image { get; set; }
    }
}
