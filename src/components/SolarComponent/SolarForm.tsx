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
    <form onSubmit={handleSubmit} className="space-y-4">
      <div className="flex flex-col space-y-2">
        <label htmlFor="city" className="text-sm font-medium text-gray-700">City</label>
        <input
          type="text"
          id="city"
          className="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all"
          value={city}
          onChange={(e) => setCity(e.target.value)}
          placeholder="e.g. London, Paris, New York"
          required
        />
      </div>
      <div className="flex flex-col space-y-2">
        <label htmlFor="date" className="text-sm font-medium text-gray-700">Date (optional)</label>
        <input
          type="date"
          id="date"
          className="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all"
          value={date}
          onChange={(e) => setDate(e.target.value)}
        />
      </div>
      <button 
        type="submit" 
        disabled={isLoading}
        className={`w-full py-2 px-4 rounded-md text-white font-semibold shadow-sm transition-all
          ${isLoading 
            ? 'bg-blue-300 cursor-not-allowed' 
            : 'bg-blue-600 hover:bg-blue-700 active:bg-blue-800'
          }`}
      >
        {isLoading ? (
          <span className="flex items-center justify-center gap-2">
            <svg className="animate-spin h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
              <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
              <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
            </svg>
            Loading...
          </span>
        ) : 'Get Solar Data'}
      </button>
    </form>
  );
};

export default SolarForm;
