import type { SolarDto } from '../../types';

interface SolarResponseProps {
  data: SolarDto | null;
  error: string | null;
  city: string;
  date: string;
}

const SolarResponse = ({ data, error, city, date }: SolarResponseProps) => {
  if (error) {
    return (
      <div className="bg-red-50 text-red-700 p-4 rounded-md border border-red-200">
        <p className="font-medium">Error</p>
        <p className="text-sm">{error}</p>
      </div>
    );
  }

  if (!data) {
    return null;
  }

  return (
    <div className="space-y-4">
      <div className="border-b border-gray-200 pb-2">
        <h3 className="text-xl font-bold text-gray-800 capitalize">{city}</h3>
        <p className="text-sm text-gray-500">{date}</p>
      </div>
      <div className="grid grid-cols-2 gap-4">
        <div className="bg-orange-50 p-4 rounded-lg border border-orange-100 flex flex-col items-center">
          <span className="text-orange-600 font-medium text-sm uppercase tracking-wide">Sunrise</span>
          <span className="text-2xl font-bold text-gray-800 mt-1">{data.sunrise}</span>
        </div>
        <div className="bg-indigo-50 p-4 rounded-lg border border-indigo-100 flex flex-col items-center">
          <span className="text-indigo-600 font-medium text-sm uppercase tracking-wide">Sunset</span>
          <span className="text-2xl font-bold text-gray-800 mt-1">{data.sunset}</span>
        </div>
      </div>
    </div>
  );
};

export default SolarResponse;
