// Libraries
import { useEffect, useState } from 'react';
import { Button, Form } from 'react-bootstrap';
import { Link, Navigate, useNavigate, useParams } from 'react-router-dom';

// App's features
import { decode, isInteger } from '../../../utils';
import { createTag, getTagById, updateTag } from '../../../services/tags';

export default function Edit() {
	// Hooks
	const navigate = useNavigate();

	const initialState = {
		id: 0,
		name: '',
		urlSlug: '',
		description: '',
	};

	const [tag, setTag] = useState(initialState);
	const [validated, setValidated] = useState(false);

	const { id } = useParams();

	const handleSubmit = async (e) => {
		e.preventDefault();

		if (e.currentTarget.checkValidity() === false) {
			e.stopPropagation();
			setValidated(true);
		} else {
			let isSuccess = true;
			if (id > 0) {
				const data = await updateTag(id, tag);
				if (!data.isSuccess) isSuccess = false;
			} else {
				const data = await createTag(tag);
				if (!data.isSuccess) isSuccess = false;
			}
			if (isSuccess) alert('Đã lưu thành công!');
			else alert('Đã xảy ra lỗi!');
			navigate('/admin/tags');
		}
	};

	useEffect(() => {
		document.title = 'Thêm/cập nhật thẻ';

		fetchTag();

		async function fetchTag() {
			const data = await getTagById(id);
			if (data) setTag(data);
			else setTag(initialState);
		}
		// eslint-disable-next-line
	}, [id]);

	if (id && !isInteger(id))
		return <Navigate to='/400?redirectTo=/admin/tags' />;

	return (
		<>
			<h1 className='px-4 py-3 text-danger'>Thêm/cập nhật thẻ</h1>
			<Form
				className='mb-5 px-4'
				onSubmit={handleSubmit}
				noValidate
				validated={validated}
			>
				<Form.Control type='hidden' name='id' value={tag.id} />
				<div className='row mb-3'>
					<Form.Label className='col-sm-2 col-form-label'>
						Thẻ
					</Form.Label>
					<div className='col-sm-10'>
						<Form.Control
							type='text'
							name='name'
							required
							value={tag.name || ''}
							onChange={(e) =>
								setTag({
									...tag,
									name: e.target.value,
								})
							}
						/>
						<Form.Control.Feedback type='invalid'>
							Không được bỏ trống
						</Form.Control.Feedback>
					</div>
				</div>
				<div className='row mb-3'>
					<Form.Label className='col-sm-2 col-form-label'>
						Slug
					</Form.Label>
					<div className='col-sm-10'>
						<Form.Control
							type='text'
							name='urlSlug'
							title='Url slug'
							value={tag.urlSlug || ''}
							onChange={(e) =>
								setTag({
									...tag,
									urlSlug: e.target.value,
								})
							}
							required
						/>
						<Form.Control.Feedback type='invalid'>
							Không được bỏ trống
						</Form.Control.Feedback>
					</div>
				</div>
				<div className='row mb-3'>
					<Form.Label className='col-sm-2 col-form-label'>
						Mô tả
					</Form.Label>
					<div className='col-sm-10'>
						<Form.Control
							as='textarea'
							type='text'
							name='description'
							title='description'
							value={decode(tag.description || '')}
							onChange={(e) =>
								setTag({
									...tag,
									description: e.target.value,
								})
							}
						/>
					</div>
				</div>
				<div className='text-center'>
					<Button variant='primary' type='submit'>
						Lưu các thay đổi
					</Button>
					<Link to='/admin/tags' className='btn btn-danger ms-2'>
						Hủy và quay lại
					</Link>
				</div>
			</Form>
		</>
	);
}
