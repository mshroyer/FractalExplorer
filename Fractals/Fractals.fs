#light

module FractalExplorer.Fractals

open System.Windows.Media.Imaging

type Membership =
    | Member
    | Escaped of int

type ColorRGB = { Red: int; Green: int; Blue: int }

type System.Windows.Media.Imaging.WriteableBitmap with
    member this.SetPixelColor(x, y, c) =
        let colorToInt c =
            (255<<<24)|||(c.Red<<<16)|||(c.Green<<<8)|||(c.Blue)
        
        let w = this.PixelWidth
        this.Pixels.SetValue(colorToInt c, x + w * y)

let linearGradient startColor endColor p =
    { Red   = int (p * (float startColor.Red)   + (1.0-p) * (float endColor.Red));
      Green = int (p * (float startColor.Green) + (1.0-p) * (float endColor.Green));
      Blue  = int (p * (float startColor.Blue)  + (1.0-p) * (float endColor.Blue)) }

//let grad = linearGradient { Red = 22; Green = 23; Blue = 61 } { Red = 255; Green = 255; Blue = 255 }
let grad = linearGradient { Red = 255; Green = 255; Blue = 255; } { Red = 32; Green = 32; Blue = 32; }

let WriteMandelbrot (bmp : WriteableBitmap) =
    let w = bmp.PixelWidth
    let h = bmp.PixelHeight

    let mValue (x0 : float) (y0 : float) =
        let rec mIter i (x : float) (y : float) = 
            match i with
            | 0 -> Member
            | _ -> if x*x + y*y > float 4
                   then Escaped(i)
                   else mIter (i-1) (x0 + x*x - y*y) (y0 + 2.*x*y)
        mIter 63 x0 y0

    let valueColor v = 
        match v with
        | Member     -> { Red = 0; Green = 0; Blue = 0 }
        | Escaped(n) -> grad ((float (n*n)) / 4096.)

    let drawRow y =
        for x = 0 to (w-1) do
            let v = mValue (4.*(float x)/(float w)-2.5) (3.*(float y)/(float h)-1.5)
            bmp.SetPixelColor(x, y, valueColor v)

    Async.Parallel [ for y in 0 .. (h-1) -> async { drawRow y } ]
    |> Async.RunSynchronously
    |> ignore
