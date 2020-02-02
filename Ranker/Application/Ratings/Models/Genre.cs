using System.Runtime.Serialization;

namespace Ranker.Application.Ratings.Models
{
    [DataContract(Name = "Genre", Namespace = "")]
    public sealed class Genre
    {
        [DataMember]
        public string Name { get; set; } = string.Empty;
    }
}
