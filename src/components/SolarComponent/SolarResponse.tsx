import type { SolarDto } from '../../types';

interface SolarResponseProps {
  data: SolarDto | null;
  error: string | null;
  city: string;
  date: string;
}

const SolarResponse = ({ data, error, city, date }: SolarResponseProps) => {
  if (error) {
    return <div className="solar-response error">{error}</div>;
  }

  if (!data) {
    return null;
  }

  return (
    <div className="solar-response">
      <h3>Data for {city}</h3>
      <div className="response-details">
        <p><strong>Date:</strong> {date}</p>
        <p><strong>Sunrise:</strong> {data.sunrise}</p>
        <p><strong>Sunset:</strong> {data.sunset}</p>
      </div>
    </div>
  );
};

export default SolarResponse;
