import { Link } from "react-router-dom"

function HomePage() {
  return (
    <>
      <h1>Home page</h1>
      <Link to="/solar" style={{ color: '#646cff', textDecoration: 'underline' }}>
        Click here to go to query page.
      </Link>
    </>
  )
}

export default HomePage
