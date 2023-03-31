import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPhone, faEnvelope } from '@fortawesome/free-solid-svg-icons';
import { faCode } from '@fortawesome/free-solid-svg-icons';

export default function Contact() {
	return (
		<div className='p-5'>
			<h2 className='mb-5 text-center text-uppercase'>Liên hệ</h2>
			<div className='grid'>
				<div className='row p-5 border border-1 border-gray rounded'>
					<div className='col-4'>
						<div className='mb-3 fs-5'>Thông tin liên hệ:</div>
						<div>
							<FontAwesomeIcon icon={faPhone} />
							<a href='tel:0123456789' className='ms-2'>
								0123.456.789
							</a>
						</div>
						<div className='my-2'>
							<FontAwesomeIcon icon={faEnvelope} />
							<a
								href='mailto:2011401@dlu.edu.vn'
								className='ms-2'
							>
								2011401@dlu.edu.vn
							</a>
						</div>
						<div>
							<FontAwesomeIcon icon={faCode} />
							<a
								href='https://github.com/tranbaolam26902'
								target='_blank'
								className='ms-2'
							>
								Github
							</a>
						</div>
					</div>
					<div className='col-8'>
						<form>
							<div className='mb-3 fs-3 text-center text-uppercase'>
								Gửi ý kiến
							</div>
							<div className='mb-3'>
								<div className='d-flex justify-content-between'>
									<label
										htmlFor='email'
										className='form-label'
									>
										Email
									</label>
									<span className='text-secondary fst-italic fs-6'>
										(Bắt buộc)
									</span>
								</div>
								<input
									type='email'
									name='email'
									className='form-control'
									id='email'
									required
								/>
							</div>
							<div className='mb-3'>
								<div className='d-flex justify-content-between'>
									<label
										htmlFor='subject'
										className='form-label'
									>
										Chủ đề
									</label>
									<span className='text-secondary fst-italic fs-6'>
										(Bắt buộc)
									</span>
								</div>
								<input
									type='text'
									name='subject'
									className='form-control'
									id='subject'
									required
								/>
							</div>
							<div className='mb-3'>
								<div className='d-flex justify-content-between'>
									<label
										htmlFor='content'
										className='form-label'
									>
										Nội dung
									</label>
									<span className='text-secondary fst-italic fs-6'>
										(Bắt buộc)
									</span>
								</div>
								<textarea
									className='form-control'
									name='content'
									id='content'
									rows='8'
									required
								></textarea>
							</div>
							<button
								type='submit'
								className='btn btn-primary px-5 py-2'
							>
								Gửi
							</button>
						</form>
					</div>
				</div>
			</div>
			<iframe
				src='https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d681.7835211958258!2d108.44565228640165!3d11.956223942437685!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x317112db826dc281%3A0x7a577a72a3694368!2zS2hvYSBDw7RuZyBuZ2jhu4cgVGjDtG5nIHRpbg!5e0!3m2!1svi!2s!4v1658199935564!5m2!1svi!2s'
				className='mt-5 w-100'
				height='512px'
				style={{ border: 0 }}
				allowFullScreen=''
				loading='lazy'
				referrerPolicy='no-referrer-when-downgrade'
			></iframe>
		</div>
	);
}
