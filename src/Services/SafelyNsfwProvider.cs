using Discord;
using Discord.Plugin.Utils;
using DLsiteSearch;
using DLsiteSearch.Entities;

namespace Citcord.Services;

public sealed class SafelyNsfwProvider
{
    public static Embed CreateById(DLsiteProduct product, IMessage message)
    {
        var builder = new EmbedBuilder();

        if (product.Rating is DLsiteProductRating.R15 or DLsiteProductRating.R18)
        {
            if (message.Channel is ITextChannel {IsNsfw: true})
            {
                OutputData(builder, product);
            }
            else
            {
                builder.Description = "この作品は年齢制限コンテンツなのでnsfwチャンネルでのみ表示できます。";
            }
        }
        else
        {
            OutputData(builder, product);
        }
        
        return builder.Build();
    }

    private static void OutputData(EmbedBuilder builder, DLsiteProduct product)
    {
        OutputAuthor(builder, product);   
        
        OutputDescription(builder, product);
        
        OutputMaker(builder, product);
        
        OutputPrice(builder, product);
        
        OutputReleaseDate(builder, product);
        
        OutputRate(builder, product);
        
        OutputType(builder, product);
        
        OutputGenre(builder, product);
        
        OutputStoryWriter(builder, product);
        
        OutputIllustrator(builder, product);
        
        OutputCharacterVoice(builder, product);
        
        OutputMusic(builder, product);
    }

    private static void OutputAuthor(EmbedBuilder builder, DLsiteProduct product)
    {
        builder.Author = new EmbedAuthorBuilder
        {
            Name = product.Name,
            Url = product.Uri
        };
    }
    
    private static void OutputDescription(EmbedBuilder builder, DLsiteProduct product)
    {
        builder.Description = product.Description.Length < 100 ? product.Description : $"{product.Description[..100]}...";
    }
    
    private static void OutputMaker(EmbedBuilder builder, DLsiteProduct product)
    {
        if (String.IsNullOrEmpty(product.Maker.Key))
        {
            builder.WithFields(new EmbedFieldBuilder
            {
                Name = "Maker",
                Value = "undefined",
                IsInline = true
            });
        }
        else if (product.Maker.Value is null)
        {
            builder.WithFields(new EmbedFieldBuilder
            {
                Name = "Maker",
                Value = product.Maker.Key,
                IsInline = true
            });
        }
        else
        {
            builder.WithFields(new EmbedFieldBuilder
            {
                Name = "Maker",
                Value = Markdown.DirectLink(product.Maker.Key, product.Maker.Value),
                IsInline = true
            });
        }
    }

    private static void OutputPrice(EmbedBuilder builder, DLsiteProduct product)
    {
        builder.WithFields(new EmbedFieldBuilder
        {
            Name = "Price",
            Value = $"{(product.FixedPrice is null ? $"{product.Price:#,0}" : $"{product.Price:#,0} -> {product.FixedPrice:#,0}")}",
            IsInline = true
        });
    }

    private static void OutputReleaseDate(EmbedBuilder builder, DLsiteProduct product)
    {
        builder.WithFields(new EmbedFieldBuilder
        {
            Name = "Release Date",
            Value = $"<t:{product.ReleaseDate.ToUnixTimeSeconds()}:d>",
            IsInline = true
        });
    }
    
    private static void OutputRate(EmbedBuilder builder, DLsiteProduct product)
    {
        builder.WithFields(new EmbedFieldBuilder
        {
            Name = "Rate",
            Value = product.Rating,
            IsInline = true
        });
    }
    
    private static void OutputType(EmbedBuilder builder, DLsiteProduct product)
    {
        builder.WithFields(new EmbedFieldBuilder
        {
            Name = "Type",
            Value = product.Type,
            IsInline = true
        });
    }

    private static void OutputGenre(EmbedBuilder builder, DLsiteProduct product)
    {
        if (product.GenreCodes.Count != 0)
        {
            builder.WithFields(new EmbedFieldBuilder
            {
                Name = "Genre",
                Value = String.Join(", ", product.GenreCodes.Select(x => x.Name)),
                IsInline = true
            });
        }
    }

    private static void OutputStoryWriter(EmbedBuilder builder, DLsiteProduct product)
    {        
        if (product.StoryWriter.Count != 0)
        {
            builder.WithFields(new EmbedFieldBuilder
            {
                Name = "StoryWriter",
                Value = String.Join(" ", product.StoryWriter.Select(x => 
                    x.Value is null ? x.Key : $"{Markdown.DirectLink(x.Key, x.Value)}")),
                IsInline = true
            });
        }
    }

    private static void OutputIllustrator(EmbedBuilder builder, DLsiteProduct product)
    {        
        if (product.Illustrator.Count != 0)
        {
            builder.WithFields(new EmbedFieldBuilder
            {
                Name = "Illustrator",
                Value = String.Join(" ", product.Illustrator.Select(x => 
                    x.Value is null ? x.Key : $"{Markdown.DirectLink(x.Key, x.Value)}")),
                IsInline = true
            });
        }
    }

    private static void OutputCharacterVoice(EmbedBuilder builder, DLsiteProduct product)
    {
        if (product.CharacterVoice.Count != 0)
        {
            builder.WithFields(new EmbedFieldBuilder
            {
                Name = "CV",
                Value = String.Join(" ", product.CharacterVoice.Select(x =>
                    x.Value is null ? x.Key : $"{Markdown.DirectLink(x.Key, x.Value)}")),
                IsInline = true
            });
        }
    }

    private static void OutputMusic(EmbedBuilder builder, DLsiteProduct product)
    {
        if (product.Music.Count != 0)
        {
            builder.WithFields(new EmbedFieldBuilder
            {
                Name = "Music",
                Value = String.Join(" ", product.Music.Select(x => 
                    x.Value is null ? x.Key : $"{Markdown.DirectLink(x.Key, x.Value)}")),
                IsInline = true
            });
        }
    }
}