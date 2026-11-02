1-نجهز اولا مجلد الفتشر وبداخله مجلد الكوماندز والكويريز
2-في الاكويري نحتاج مجلدين  مجلد خاص بجلب كل الكائنات ومجلد خاص بجلب كائن واحد عن طريق الايدي مثلا
3- في كل مجلد من هذم الاثنين نحتاج ثلاثة ملفات 
FolderNamesQuery
FolderNamesQueryHandler
FolderNameDto
4- في ال FolderNamesQueryHandler نحتاج نعمل حقن لثلاثة كائنات 
Mapper,Logger,Repository
5- في داخل دالة ال handle نقوم بالاتي
 //Query the database
 var leaveTypes =await _leaveTypeRepository.GetAllAsync();
  
 //Convert the data objects to DTO objects
 var data = _mapper.Map<List<LeaveTypeDto>>(leaveTypes);

 //logging the information that the leave types were retrieved successfully
 _logger.LogInformation("LeaveTypes were retrieved successfully");

 //return List of DTO objects
 return data;

 5- نروح نيضف التحويلات التالية لمجلد البروفايلات
 CreateMap<LeaveTypeDto, LeaveType>().ReverseMap(); // tow ridercations from LeaveTypeDto to LeaveType and vice versa
CreateMap<LeaveType, LeaveTypeDetailsDto>(); // one direction from LeaveType to LeaveTypeDetailsDto
CreateMap<CreateLeaveTypeCommand, LeaveType>();
CreateMap<UpdateLeaveTypeCommand, LeaveType>();

6- بالنسبة للكويري التي تجلب كائن واحد بالايدي فاننا نضع خاصية الايدي في ملف الكويري وبقية الخصائص في ملف ال Dto
