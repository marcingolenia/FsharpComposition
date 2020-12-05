namespace Api
module Views =
  open Giraffe.ViewEngine

  let index =
    html [] [
      head [] [ title [] [ encodedText "Whatever Api" ] ]
      body [] [ h1 [] [ encodedText "Whatever Api" ] ]
    ]
