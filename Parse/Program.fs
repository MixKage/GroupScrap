open FSharp.Data
open System.Text.RegularExpressions

[<EntryPoint>]
let main(args) =
    let path = args.[0]

    let fixWeb = 
        let f = Regex.Replace(System.IO.File.ReadAllText(path), "<head>[\\s\\S\\n]*</head>","")
        System.IO.File.WriteAllText(path, f)

    fixWeb

    let webinar = HtmlDocument.Load(path)

    let webinar_div =
        webinar.Descendants ["div"]
        |> Seq.choose (fun x ->
               x.TryGetAttribute("class")
               |> Option.map (fun a -> x.InnerText(), a.Value())
        )
        |> Seq.toList

    let names = 
        webinar_div 
        |> List.filter(fun (x, y) -> y.Contains("UserRow__name_"))
        |> List.map(fun (x, y) -> x)

    let formatNames fileName =
        use file = System.IO.File.AppendText(fileName)
        names
        |> List.iter (fun elem -> fprintf file "%s\n" elem)

    formatNames "generalList.txt"
    0