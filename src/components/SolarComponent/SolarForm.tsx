import { useState } from 'react';

interface SolarFormProps {
  onSearch: (city: string, date?: string) => void;
  isLoading: boolean;
}

const SolarForm = ({ onSearch, isLoading }: SolarFormProps) => {
  const [city, setCity] = useState('');
  const [date, setDate] = useState('');

  const handleSubmit = (e: React.SubmitEvent) => {
    e.preventDefault();
    if (city.trim()) {
      onSearch(city, date || undefined);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="solar-form">
      <div className="form-group">
        <label htmlFor="city">City:</label>
        <input
          type="text"
          id="city"
          value={city}
          onChange={(e) => setCity(e.target.value)}
          placeholder="Enter city name"
          required
        />
      </div>
      <div className="form-group">
        <label htmlFor="date">Date (optional):</label>
        <input
          type="date"
          id="date"
          value={date}
          onChange={(e) => setDate(e.target.value)}
        />
      </div>
      <button type="submit" disabled={isLoading}>
        {isLoading ? 'Loading...' : 'Get Solar Data'}
      </button>
    </form>
  );
};

export default SolarForm;
