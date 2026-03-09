import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import Login from './pages/Login';
import Register from './pages/Register';
import Products from './pages/Products';
import CategoryList from './pages/CategoryList';
import Navbar from './components/Navbar';

function App() {
  const isAuthenticated = !!localStorage.getItem('token');

  return (
    <BrowserRouter>
      <Navbar />
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/registro" element={<Register />} />
        <Route path="/productos" element={<Products />} />

        <Route
          path="/categories"
          element={isAuthenticated ? <CategoryList /> : <Navigate to="/login" replace />}
        />

        <Route path="*" element={<Navigate to="/productos" replace />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;