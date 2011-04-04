#light

module FractalExplorer.Fractals

open Microsoft.FSharp.Math
open System.Windows.Media.Imaging

let WriteMandelbrot (bmp : WriteableBitmap) =
    let w = bmp.PixelWidth
    let h = bmp.PixelHeight

    let mValue (z0 : complex) =
        let rec mIter i (z : complex) = 
            match i with
            | 0 -> 0
            | _ -> if (Complex.magnitude z) > float 2
                   then i
                   else mIter (i-1) (z*z + z0)
        mIter 31 z0

    let pixelToZ x y =
        complex (4.*(float x)/(float w)-2.5) (3.*(float y)/(float h)-1.5)

    let drawRow y =
        for x = 0 to (w-1) do
            let v = 8 * (mValue <| pixelToZ x y)
            bmp.Pixels.SetValue((255<<<24)|||(v<<<16)|||(v<<<8)|||v, x+y*w)

    Async.Parallel [ for y in 0 .. (h-1) -> async { drawRow y } ]
    |> Async.RunSynchronously
    |> ignore
