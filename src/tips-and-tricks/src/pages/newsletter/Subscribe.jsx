import { useRef, useState } from 'react';

import { subscribe } from '../../services/newsletter';

export default function Subscribe() {
	// Component's refs
	const emailRef = useRef();

	// Component's states
	const [errorMessages, setErrorMessages] = useState([]);

	// Component's event handlers
	const handleSubscribe = async (e) => {
		e.preventDefault();
		const data = await subscribe(emailRef.current.value);
		if (data === true) {
			window.alert('Đăng ký thành công!');
			setErrorMessages([]);
		} else setErrorMessages(data);
	};

	return (
		<div
			className='mx-auto mt-5 p-5 rounded shadow'
			style={{ maxWidth: 576 }}
		>
			<h5 className='text-uppercase'>Đăng ký nhận thông báo</h5>
			<p>Nhận thông báo về tin tức, bài viết mới thông qua email.</p>
			<form onSubmit={handleSubscribe}>
				<div className='d-flex'>
					<input
						ref={emailRef}
						type='email'
						className='flex-fill px-2 rounded border border-1 border secondary'
						placeholder='Email của bạn'
						required
					/>
					<button
						type='submit'
						className='btn btn-primary ms-3 px-4 py-2'
					>
						Đăng ký
					</button>
				</div>
				{errorMessages.length > 0
					? errorMessages.map((errorMessage, index) => (
							<div key={index} className='mt-3 text-danger'>
								{errorMessage}
							</div>
					  ))
					: null}
			</form>
		</div>
	);
}
