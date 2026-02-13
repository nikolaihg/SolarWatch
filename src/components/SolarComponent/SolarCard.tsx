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
    <div className="solar-card">
      <SolarForm onSearch={handleSearch} isLoading={loading} />
      <SolarResponse 
        data={solarData} 
        error={error} 
        city={searchedCity}
        date={searchedDate}
      />
    </div>
  );
};

export default SolarCard;
