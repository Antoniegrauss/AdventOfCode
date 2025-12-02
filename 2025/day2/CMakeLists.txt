add_executable(day2)
target_sources(day2
    PRIVATE
    day2.cpp)
target_link_libraries(day2 read_file)
target_include_directories(day2 PRIVATE ../utils)
