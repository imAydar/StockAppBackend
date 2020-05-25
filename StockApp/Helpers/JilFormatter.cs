using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StockApp.Models;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections;
using System.Web.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Net.Http.Formatting;
using System.Text;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization;
//using System.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Jil;
public class JilFormatter : MediaTypeFormatter
{
    private static readonly MediaTypeHeaderValue _applicationJsonMediaType = new MediaTypeHeaderValue("application/json");
    private static readonly MediaTypeHeaderValue _textJsonMediaType = new MediaTypeHeaderValue("text/json");


    private readonly Options _options;

    public JilFormatter(Options options)
    {
        _options = options;
        SupportedMediaTypes.Add(_applicationJsonMediaType);
        SupportedMediaTypes.Add(_textJsonMediaType);

        SupportedEncodings.Add(new UTF8Encoding(false, true));
        SupportedEncodings.Add(new UnicodeEncoding(false, true, true));
    }



    public override bool CanReadType(Type type)
    {
        if (type == null)
            return false;
        return true;
    }

    public override bool CanWriteType(Type type)
    {
        if (type == null)
            return false;
        return true;
    }

    public override Task<object> ReadFromStreamAsync(Type type, Stream input, HttpContent content, IFormatterLogger formatterLogger)
    {
        var reader = new StreamReader(input);
        var deserialize = TypedDeserializers.GetTyped(type);
        var result = deserialize(reader, _options);
        return Task.FromResult(result);
    }

    public override Task WriteToStreamAsync(Type type, object value, Stream output, HttpContent content, TransportContext transportContext)
    {
        var writer = new StreamWriter(output);
        JSON.Serialize(value, writer, _options);
        writer.Flush();
        return Task.FromResult(true);
    }
}
static class TypedDeserializers
{
    private static readonly ConcurrentDictionary<Type, Func<TextReader, Options, object>> _methods;
    private static readonly MethodInfo _method = typeof(JSON).GetMethod("Deserialize", new[] { typeof(TextReader), typeof(Options) });

    static TypedDeserializers()
    {
        _methods = new ConcurrentDictionary<Type, Func<TextReader, Options, object>>();
    }

    public static Func<TextReader, Options, object> GetTyped(Type type)
    {
        return _methods.GetOrAdd(type, CreateDelegate);
    }

    private static Func<TextReader, Options, object> CreateDelegate(Type type)
    {
        return (Func<TextReader, Options, object>)_method
            .MakeGenericMethod(type)
            .CreateDelegate(typeof(Func<TextReader, Options, object>));
    }
}