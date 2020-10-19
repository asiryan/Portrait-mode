using System;
using System.Drawing;
using UMapx.Core;
using UMapx.Imaging;

namespace portrait_mode
{
    /// <summary>
    /// Defines "portrait mode" filter.
    /// </summary>
    public class PortraitModeFilter
    {
        #region Private data
        BoxBlur _boxBlur;
        AlphaChannelFilter _alphaChannelFilter;
        Merge _merge;
        double _strength;
        #endregion

        #region Class components
        /// <summary>
        /// Initializes "portrait mode" filter.
        /// </summary>
        /// <param name="strength">Strength</param>
        public PortraitModeFilter(double strength)
        {
            _boxBlur = new BoxBlur();
            _alphaChannelFilter = new AlphaChannelFilter();
            _merge = new Merge(0, 0, 255);
            _strength = strength;
        }
        /// <summary>
        /// Gets or sets strength.
        /// </summary>
        public double Strength
        {
            get
            {
                return _strength;
            }
            set
            {
                _strength = Maths.Double(value);
            }
        }
        /// <summary>
        /// Applies filter to image.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="mask">Segmentation mask</param>
        /// <returns>Portrait image</returns>
        public Bitmap Apply(Bitmap image, Bitmap mask)
        {
            // time
            var tic = Environment.TickCount;
            Console.WriteLine("Applying portrait mode filter...");

            // deep person lab
            var alphaMask = (Bitmap)image.Clone();
            var portrait = (Bitmap)image.Clone();
            var segmentantionMask = (Bitmap)mask.Clone();

            // radius calculation
            var radius = (int)(_strength * 2 * (( Math.Max(image.Height, image.Width) / 100 ) + 1));
            Console.WriteLine($"Blur radius --> {radius}");

            // gaussian blur approximation
            _boxBlur.Size = new SizeInt(radius, radius);
            _boxBlur.Apply(portrait);
            _boxBlur.Apply(segmentantionMask);

            _boxBlur.Size = new SizeInt(radius / 2, radius / 2);
            _boxBlur.Apply(portrait);
            _boxBlur.Apply(segmentantionMask);

            // merging images
            _alphaChannelFilter.Apply(alphaMask, segmentantionMask);
            _merge.Apply(portrait, alphaMask);
            alphaMask.Dispose();
            segmentantionMask.Dispose();
            Console.WriteLine($"Portrait mode filter was applied in {Environment.TickCount - tic} mls.");

            return portrait;
        }
        #endregion
    }
}
