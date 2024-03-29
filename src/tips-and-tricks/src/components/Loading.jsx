import { Button, Spinner } from 'react-bootstrap';

export default function Loading() {
	return (
		<div className='text-center'>
			<Button variant='outline-success' disabled className='border-none'>
				<Spinner
					as='span'
					animation='grow'
					size='sm'
					role='status'
					aria-hidden='true'
				/>
				&nbsp;Đang tải...
			</Button>
		</div>
	);
}
