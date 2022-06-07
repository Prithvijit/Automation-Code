using System;
using System.Collections.Generic;

public class SimpleMovingAverage
	{
		List<double> samples;
		protected double total;
        private int sampleSize;
        private int totalProcessed;

		/// <summary>
		/// Get the average for the current number of samples.
		/// </summary>
		public double Average
		{
			get
			{
				if (samples.Count == 0)
				{
					throw new ApplicationException("Number of samples is 0.");
				}

                if (totalProcessed <= (sampleSize))
                {
                    return samples[samples.Count - 1]; //rlm 19-07-23
                }
                else
                {
                    return total / sampleSize;
                }
			}
		}

    /// <summary>
    /// Get the count of samples loaded in buffer
    /// </summary>
    public int Count
    {
        get
        {
            return samples.Count;
        }
    }

    /// <summary>
    /// Constructor, initializing the sample size to the specified number.
    /// </summary>
    public SimpleMovingAverage(int numSamples)
	{
		if (numSamples <= 0)
		{
			throw new ArgumentOutOfRangeException("numSamples can't be negative or 0.");
		}

        sampleSize = numSamples;
        samples = new List<double>();
        totalProcessed = 0;
    }

    /// <summary>
    /// Adds a sample to the sample collection.
    /// </summary>
    public void AddSample(double val)
	{
        if (totalProcessed > 0)
            total += val; //all but the first

        if (samples.Count > (sampleSize))
        {
            samples.RemoveAt(0); //remove oldest
            total -= samples[0];
        }

        samples.Add(val);
        totalProcessed++;
	}

	/// <summary>
	/// Clears all samples to 0.
	/// </summary>
	public void ClearSamples()
	{
        total = 0;
        //totalProcessed = 0;
		samples.Clear();
	}

}
