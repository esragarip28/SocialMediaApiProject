using System;
using Newtonsoft.Json;


namespace DemoApplication1.ResultJsonClass
{
    public class CloudinaryResult
    {

    [JsonProperty("width")]
    public long Width { get; set; }

    [JsonProperty("height")]
    public long Height { get; set; }

    [JsonProperty("exif")]
    public object Exif { get; set; }

    [JsonProperty("image_metadata")]
    public object ImageMetadata { get; set; }

    [JsonProperty("faces")]
    public object Faces { get; set; }

    [JsonProperty("quality_analysis")]
    public object QualityAnalysis { get; set; }

    [JsonProperty("quality_score")]
    public long QualityScore { get; set; }

    [JsonProperty("colors")]
    public object Colors { get; set; }

    [JsonProperty("phash")]
    public object Phash { get; set; }

    [JsonProperty("delete_token")]
    public object DeleteToken { get; set; }

    [JsonProperty("info")]
    public object Info { get; set; }

    [JsonProperty("pages")]
    public long Pages { get; set; }

    [JsonProperty("context")]
    public object Context { get; set; }

    [JsonProperty("illustration_score")]
    public long IllustrationScore { get; set; }

    [JsonProperty("semi_transparent")]
    public bool SemiTransparent { get; set; }

    [JsonProperty("grayscale")]
    public bool Grayscale { get; set; }

    [JsonProperty("eager")]
    public object Eager { get; set; }

    [JsonProperty("predominant")]
    public object Predominant { get; set; }

    [JsonProperty("cinemagraph_analysis")]
    public object CinemagraphAnalysis { get; set; }

    [JsonProperty("accessibility_analysis")]
    public object AccessibilityAnalysis { get; set; }

    [JsonProperty("signature")]
    public string Signature { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("resource_type")]
    public string ResourceType { get; set; }

    [JsonProperty("moderation")]
    public object Moderation { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonProperty("tags")]
    public object[] Tags { get; set; }

    [JsonProperty("access_control")]
    public object AccessControl { get; set; }

    [JsonProperty("access_mode")]
    public object AccessMode { get; set; }

    [JsonProperty("etag")]
    public string Etag { get; set; }

    [JsonProperty("placeholder")]
    public bool Placeholder { get; set; }

    [JsonProperty("original_filename")]
    public string OriginalFilename { get; set; }

    [JsonProperty("public_id")]
    public string PublicId { get; set; }

    [JsonProperty("version")]
    public long Version { get; set; }

    [JsonProperty("url")]
    public Uri Url { get; set; }

    [JsonProperty("secure_url")]
    public Uri SecureUrl { get; set; }

    [JsonProperty("bytes")]
    public long Bytes { get; set; }

    [JsonProperty("format")]
    public string Format { get; set; }

    [JsonProperty("metadata")]
     public object Metadata { get; set; }

    [JsonProperty("error")]
    public object Error { get; set; }
    }
}
