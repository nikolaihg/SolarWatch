import { useState } from "react"
import { getToken } from "../api/auth"

function HomePage() {
  const [count, setCount] = useState(0)
  const token = getToken()

  return (
    <>
      <h1>Solar Watch</h1>
      <div className="card">
        <button onClick={() => setCount((count) => count + 1)}>
          count is {count}
        </button>
        <p><strong>Token:</strong></p>
        <p style={{ wordBreak: 'break-all', fontSize: '0.9em' }}>{token}</p>
      </div>
    </>
  )
}

export default HomePage
