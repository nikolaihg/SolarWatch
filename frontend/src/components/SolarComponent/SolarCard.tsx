import { useState } from 'react';
import SolarForm from './SolarForm';
import SolarResponse from './SolarResponse';
import { getSolarWatch } from '../../api/solarwatch';
import type { SolarDto } from '../../types';

const SolarCard = () => {
  const [solarData, setSolarData] = useState<SolarDto | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [searchedCity, setSearchedCity] = useState<string>('');
  const [searchedDate, setSearchedDate] = useState<string>('');

  const handleSearch = async (city: string, date?: string) => {
    setLoading(true);
    setError(null);
    setSolarData(null);
    setSearchedCity(city);
    setSearchedDate(date || new Date().toISOString().split('T')[0]);
    
    try {
      const data = await getSolarWatch(city, date);
      setSolarData(data);
    } catch (err) {
      console.error(err);
      setError('Failed to fetch solar data. Please check the city name and try again.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="bg-white rounded-xl shadow-xl border border-gray-100 relative">
      <div className="p-6">
        <SolarForm onSearch={handleSearch} isLoading={loading} />
      </div>
      {(solarData || error) && (
        <div className="bg-gray-50 p-6 border-t border-gray-100 rounded-b-xl">
          <SolarResponse 
            data={solarData} 
            error={error} 
            city={searchedCity}
            date={searchedDate}
          />
        </div>
      )}
    </div>
  );
};

export default SolarCard;
