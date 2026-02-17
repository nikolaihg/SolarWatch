import { useState, useEffect, useRef } from 'react';
import { getCityNames } from '../../api/solarwatch';

interface SolarFormProps {
  onSearch: (city: string, date?: string) => void;
  isLoading: boolean;
}

const SolarForm = ({ onSearch, isLoading }: SolarFormProps) => {
  const [city, setCity] = useState('');
  const [date, setDate] = useState('');
  const [suggestions, setSuggestions] = useState<string[]>([]);
  const [allCities, setAllCities] = useState<string[]>([]);
  const [showSuggestions, setShowSuggestions] = useState(false);
  const wrapperRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    getCityNames().then(cities => {
      if (Array.isArray(cities)) {
        setAllCities(cities.map(c => c.name));
      }
    }).catch(console.error);
  }, []);

  useEffect(() => {
    function handleClickOutside(event: MouseEvent) {
      if (wrapperRef.current && !wrapperRef.current.contains(event.target as Node)) {
        setShowSuggestions(false);
      }
    }
    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, [wrapperRef]);

  const handleCityChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setCity(value);
    
    if (value.length > 0) {
      const filtered = allCities.filter(c => 
        c.toLowerCase().includes(value.toLowerCase())
      );
      const sorted = filtered.sort((a, b) => {
          const aStarts = a.toLowerCase().startsWith(value.toLowerCase());
          const bStarts = b.toLowerCase().startsWith(value.toLowerCase());
          if (aStarts && !bStarts) return -1;
          if (!aStarts && bStarts) return 1;
          return a.localeCompare(b);
      });
      setSuggestions(sorted);
      setShowSuggestions(true);
    } else {
      setSuggestions([]);
      setShowSuggestions(false);
    }
  };

  const handleSuggestionClick = (suggestion: string) => {
    setCity(suggestion);
    setSuggestions([]);
    setShowSuggestions(false);
  };

  const handleSubmit = (e: React.SubmitEvent) => {
    e.preventDefault();
    if (city.trim()) {
      onSearch(city, date || undefined);
      setShowSuggestions(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <div className="flex flex-col space-y-2 relative" ref={wrapperRef}>
        <label htmlFor="city" className="text-sm font-medium text-gray-700">City</label>
        <div className="relative">
          <input
            type="text"
            id="city"
            className="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all"
            value={city}
            onChange={handleCityChange}
            onFocus={() => {
              if (city.length > 0 && suggestions.length > 0) setShowSuggestions(true);
            }}
            placeholder="e.g. London, Paris, New York"
            required
            autoComplete="off"
          />
          {showSuggestions && suggestions.length > 0 && (
            <ul className="absolute z-10 w-full bg-white border border-gray-300 rounded-md shadow-lg max-h-60 overflow-auto mt-1 left-0">
              {suggestions.map((suggestion, index) => (
                <li
                  key={index}
                  onClick={() => handleSuggestionClick(suggestion)}
                  className="px-4 py-2 hover:bg-blue-50 cursor-pointer text-sm text-gray-700 transition-colors"
                >
                  {suggestion}
                </li>
              ))}
            </ul>
          )}
        </div>
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
