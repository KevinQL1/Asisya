import { Link, useNavigate } from 'react-router-dom';

const Navbar = () => {
    const navigate = useNavigate();
    const token = localStorage.getItem('token');

    const handleLogout = () => {
        localStorage.removeItem('token');
        navigate('/login');
    };

    return (
        <nav style={{ 
            backgroundColor: '#333', 
            color: 'white', 
            padding: '0 30px', 
            display: 'flex', 
            justifyContent: 'space-between',
            alignItems: 'center',
            height: '70px',
            position: 'fixed',
            top: 0,
            left: 0,
            right: 0,
            zIndex: 3000,
            boxShadow: '0 2px 10px rgba(0,0,0,0.3)'
        }}>
            <div style={{ fontWeight: 'bold', fontSize: '1.2rem', letterSpacing: '1px' }}>
                ASISYA APP
            </div>
            
            <div style={{ display: 'flex', gap: '25px', alignItems: 'center' }}>
                <Link to="/productos" style={{ color: 'white', textDecoration: 'none', fontWeight: '500' }}>Catálogo</Link>

                {!token ? (
                    <>
                        <Link to="/login" style={{ color: 'white', textDecoration: 'none' }}>Iniciar Sesión</Link>
                        <Link to="/registro" style={{ 
                            color: 'white', 
                            textDecoration: 'none', 
                            border: '1px solid white', 
                            padding: '6px 15px', 
                            borderRadius: '4px' 
                        }}>Registrarse</Link>
                    </>
                ) : (
                    <button 
                        onClick={handleLogout} 
                        style={{ 
                            background: '#ff4d4d', 
                            border: 'none', 
                            color: 'white', 
                            padding: '8px 16px', 
                            borderRadius: '4px', 
                            cursor: 'pointer',
                            fontWeight: 'bold'
                        }}
                    >
                        Cerrar Sesión
                    </button>
                )}
            </div>
        </nav>
    );
};

export default Navbar;