import SolarCard from "../components/SolarComponent/SolarCard"

function SolarPage() {
  return (
    <div className="flex flex-col items-center">
      <h1 className="text-3xl font-bold text-gray-800 mb-8">Solar Data Search</h1>
      <div className="w-full max-w-md">
        <SolarCard/>
      </div>
    </div>
  )
}

export default SolarPage
