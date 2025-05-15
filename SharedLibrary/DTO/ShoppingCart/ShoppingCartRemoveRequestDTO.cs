using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedLibrary.DTO.ShoppingCart
{
	public class ShoppingCartRemoveRequestDTO
	{
        [JsonPropertyName("userid")]
        [Required]
        public string UserId { get; set; }

        [JsonPropertyName("productid")]
        [Required]
        public string ProductId { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
