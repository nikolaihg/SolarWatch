import { Link } from "react-router-dom"

function HomePage() {
  return (
    <div className="flex flex-col items-center justify-center min-h-[50vh] text-center">
      <h1 className="text-4xl font-extrabold text-blue-900 mb-6">Welcome to SolarWatch!</h1>
      <p className="text-xl text-gray-700 mb-8 max-w-2xl">
        Discover sunrise and sunset times for any city in the world.
      </p>
      <Link 
        to="/solar" 
        className="bg-blue-600 hover:bg-blue-700 text-white font-bold py-3 px-6 rounded-lg shadow-md transition-all transform hover:scale-105"
      >
        Go to Query Page
      </Link>
    </div>
  )
}

export default HomePage
